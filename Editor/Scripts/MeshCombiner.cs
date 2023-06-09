using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Lotec.Utils.Editor {
    public class MeshCombiner : MonoBehaviour {
        public static string simplifiedPostfix = ".Simplified";

        [UnityEditor.MenuItem("Tools/Create simplified mesh")]
        public static void CombineSelectedTransform() {
            Transform container = Selection.activeTransform;
            if (container == null) {
                Debug.LogError("A transform must be selected, that contains the objects");
                return;
            }
            GameObject go = CreateSimplifiedMesh(container);
            if (go) {
                go.transform.SetParent(container.parent);
            }
        }

        // Combine all highest LOD meshes contained in meshGroup transform
        // NOTE: meshes must use same material.
        public static GameObject CreateSimplifiedMesh(Transform meshGroup) {
            // Debug.Log($"CreateSimplifiedMesh({meshGroup.name})", meshGroup);
            List<CombineInstance> combines = new List<CombineInstance>();
            Vector3 pos = meshGroup.transform.position;
            Quaternion rot = meshGroup.transform.rotation;
            meshGroup.transform.position = Vector3.zero;
            meshGroup.transform.rotation = Quaternion.identity;
            Renderer lastRenderer = CollectHighestLodCombines(meshGroup, ref combines);
            if (combines.Count > 1) {
                Debug.Log($"Creating Simplified mesh for {meshGroup.name}. {combines.Count} submeshes", meshGroup);
                GameObject go = new GameObject(meshGroup.name + simplifiedPostfix, typeof(MeshFilter), typeof(MeshRenderer));
                go.GetComponent<MeshFilter>().sharedMesh = new Mesh();
                go.GetComponent<MeshFilter>().sharedMesh.name = meshGroup.name + simplifiedPostfix;
                go.GetComponent<MeshFilter>().sharedMesh.CombineMeshes(combines.ToArray(), true, true, true);
                go.GetComponent<MeshRenderer>().sharedMaterial = lastRenderer.sharedMaterial;
                // https://forum.unity.com/threads/combinemeshes-and-retain-lightmap-how-i-got-it-to-work.987213/
                // adding lightmap data aint working. UnityPOOP!
                // UnityEditor.Unwrapping.GenerateSecondaryUVSet(go.GetComponent<MeshFilter>().sharedMesh);
                // go.GetComponent<MeshFilter>().sharedMesh.CombineMeshes(combines.ToArray(), true, true, true);
                // go.GetComponent<MeshRenderer>().lightmapIndex = 0;
                meshGroup.transform.position = pos;
                meshGroup.transform.rotation = rot;
                go.transform.position = meshGroup.transform.position;
                go.transform.rotation = meshGroup.transform.rotation;
                return go;
            }
            return null;
        }
        // Sets up combines list with child meshes.
        // Returns last renderer
        private static Renderer CollectHighestLodCombines(Transform t, ref List<CombineInstance> combines, Transform rootTransform = null) {
            if (t.name.Contains(simplifiedPostfix)) return null;

            if (rootTransform == null) {
                rootTransform = t;
            }

            // Debug.Log($"CollectHighestLodCombines({t.name}, {combines.Count})", this);
            Renderer renderer = t.GetComponent<Renderer>();
            LODGroup lodGroup = t.GetComponent<LODGroup>();
            Transform simplifiedTransform = t.parent?.Find(t.name + simplifiedPostfix);
            if (simplifiedTransform) {
                // Use the Simplified version of the mesh if it exists.
                renderer = simplifiedTransform.GetComponent<Renderer>();
                AddToCombines(renderer, ref combines);
            } else if (lodGroup) {
                // If LodGroup, get lowest detailed mesh
                LOD[] lods = lodGroup.GetLODs();
                LOD simplest = lods[lods.Length - 1];
                foreach (Renderer r in simplest.renderers) {
                    AddToCombines(r, ref combines);
                    renderer = r;
                }
            } else {
                // Get mesh on this transform.
                AddToCombines(renderer, ref combines);
                // Search children
                for (int i = 0; i < t.childCount; i++) {
                    Renderer r = CollectHighestLodCombines(t.GetChild(i), ref combines, rootTransform);
                    if (r) {
                        renderer = r;
                    }
                }
            }
            return renderer;
        }

        private static void AddToCombines(Renderer renderer, ref List<CombineInstance> combines) {
            if (renderer) {
                Mesh mesh;
                if (renderer is SkinnedMeshRenderer) {
                    mesh = ((SkinnedMeshRenderer)renderer).sharedMesh;
                } else {
                    mesh = renderer.GetComponent<MeshFilter>().sharedMesh;
                }
                if (mesh == null) {
                    Debug.LogError($"Mesh in {renderer.name} is null!", renderer);
                } else {
                    CombineInstance combine = new CombineInstance();
                    combine.mesh = mesh;
                    combine.lightmapScaleOffset = renderer.lightmapScaleOffset;
                    combine.transform = renderer.localToWorldMatrix;
                    combines.Add(combine);
                }
            }
        }

    }
}
