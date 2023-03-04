using UnityEngine;

namespace Lotec.Utils.Extensions {
    public static class GameObjectExtensions {
        /// <summary>
        /// Get component in GameObject, or add it if it does not exist.
        /// </summary>
        /// <param name="gameObject"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component {
            T component;
            if (!gameObject.TryGetComponent<T>(out component)) {
                component = gameObject.AddComponent<T>();
            }
            return component;
        }

        /// <summary>
        /// Explicitly change GameObject active flag on all children.
        /// (SetActiveRecursively already exists in GameObject, but deprecated)
        /// TODO: Use GetChild() to skip foreach?
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="active"></param>
        public static void SetActiveRecursively(GameObject gameObject, bool active) {
            foreach (Transform transform in gameObject.transform) {
                transform.gameObject.SetActive(active);
                SetActiveRecursively(transform.gameObject, active);
            }
        }
    }
}
