using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

namespace Lotec.Utils.Packages {
    static class AddPackages {
        static RemoveRequest Request;
        static int idx = 0;
        static string[] packagesToRemove = {
            "com.unity.feature.development",
            "com.unity.visualscripting",
            "com.unity.collab-proxy",
            "com.unity.ide.rider",
            "com.unity.ide.visualstudio",
            "com.unity.test-framework",
            "com.unity.timeline",
        };
        static string[] packagesToAdd = {
            "com.unity.ide.vscode",
        };

        [MenuItem("Tools/Lotec/Set Packages")]
        static void SetPackages() {
            // Unity does NOT complete other requests if something fails???
            // Request = Client.AddAndRemove(packagesToAdd, packagesToRemove);
            RemovePackage();
            EditorApplication.update += Progress;
        }

        static void RemovePackage() {
            if (idx >= packagesToRemove.Length) {
                idx = 0;
                Client.Add(packagesToAdd[0]);
                return;
            }

            Request = Client.Remove(packagesToRemove[idx]);
            EditorApplication.update += Progress;
        }

        static void Progress() {
            if (Request.IsCompleted) {
                if (Request.Status != StatusCode.Success)
                    Debug.Log(Request.Error.message);

                EditorApplication.update -= Progress;
                idx++;
                RemovePackage();
            }
        }
    }
}
