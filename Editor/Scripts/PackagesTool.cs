using System.Linq;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

namespace Lotec.Utils.Packages {
    /// <summary>
    /// Clean up unwanted packages.
    /// </summary>
    static class AddPackages {
        static ListRequest ListRequest;
        static AddAndRemoveRequest AddAndRemoveRequest;
        static string[] _unwantedPackages = {
            "com.unity.feature.development",
            "com.unity.visualscripting",
            "com.unity.collab-proxy",
            "com.unity.ide.rider",
            "com.unity.ide.visualstudio",
            "com.unity.test-framework",
            "com.unity.timeline",
        };
        static string[] _requiredPackages = {
            "com.unity.ide.vscode",
            "com.unity.inputsystem",
            "com.unity.cinemachine",
        };

        [MenuItem("Tools/Lotec/Cleanup Packages")]
        static void SetPackages() {
            EditorApplication.update += Progress;
            ListRequest = Client.List(true);
        }

        static void Progress() {
            if (ListRequest != null) {
                HandleListRequest();
            }
            if (AddAndRemoveRequest != null) {
                HandleAddAndRemove();
            }
        }

        static void HandleListRequest() {
            if (!ListRequest.IsCompleted) return;

            if (ListRequest.Status == StatusCode.Success) {
                PackageCollection packages = ListRequest.Result;
                string[] packagesToRemove = packages
                    .Select(package => package.name)
                    .Intersect(_unwantedPackages)
                    .ToArray();
                string[] packagesToAdd = _requiredPackages
                    .Except(packages.Select(package => package.name))
                    .ToArray();
                if (packagesToRemove.Length > 0)
                    Debug.Log($"Removing packages: {System.String.Join(", ", packagesToRemove)}");
                if (packagesToAdd.Length > 0)
                    Debug.Log($"Adding packages: {System.String.Join(", ", packagesToAdd)}");
                AddAndRemoveRequest = Client.AddAndRemove(packagesToAdd, packagesToRemove);
            } else {
                Debug.Log(ListRequest.Error.message);
            }

            EditorApplication.update -= Progress;
            ListRequest = null;
        }

        static void HandleAddAndRemove() {
            if (!AddAndRemoveRequest.IsCompleted) return;

            if (AddAndRemoveRequest.Status != StatusCode.Success)
                Debug.Log(AddAndRemoveRequest.Error.message);

            EditorApplication.update -= Progress;
            AddAndRemoveRequest = null;
        }
    }
}
