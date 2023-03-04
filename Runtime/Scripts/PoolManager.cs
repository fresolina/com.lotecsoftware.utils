using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Lotec.Utils {
    /// <summary>
    /// Simple wrapper for pooling GameObjects. Automatically creates a pool per prefab.
    /// Add this script to a manager, then replace calls to Instantiate()
    ///   with PoolManager.Instantiate() and Destroy() with Poolmanager.Destroy().
    /// Do not use for prefabs that will break up into smaller pieces, etc.
    /// Ensure your pooled objects OnEnable() method resets needed values.
    /// </summary>
    public class PoolManager : MonoBehaviour {
        private static PoolManager _instance;
        // Get the pool, handling objects instantiated from a prefab
        private Dictionary<GameObject, ObjectPool<GameObject>> _poolsFromPrefab = new Dictionary<GameObject, ObjectPool<GameObject>>();
        // Get the pool, handling the instantiated object
        private Dictionary<GameObject, ObjectPool<GameObject>> _poolsFromObject = new Dictionary<GameObject, ObjectPool<GameObject>>();
        private int _poolCount;

        public static GameObject Instantiate(GameObject prefab) {
            return _instance.Get(prefab);
        }

        public static GameObject Instantiate(GameObject prefab, Vector3 position, Quaternion rotation) {
            var obj = _instance.Get(prefab);
            obj.transform.position = position;
            obj.transform.rotation = rotation;
            return obj;
        }

        public static void Destroy(GameObject obj) {
            _instance.Release(obj);
        }

        public static void Destroy(GameObject obj, float time) {
            _instance.StartCoroutine(_instance.Release(obj, time));
        }

        public GameObject Get(GameObject prefab) {
            ObjectPool<GameObject> pool;
            GameObject obj;
            if (!_poolsFromPrefab.TryGetValue(prefab, out pool)) {
                // This prefab requires a new pool for these object types.
                pool = new ObjectPool<GameObject>(
                    () => Object.Instantiate(prefab),
                    (GameObject obj) => obj.SetActive(true),
                    (GameObject obj) => obj.SetActive(false),
                    (GameObject obj) => Object.Destroy(obj)
                );
                _poolsFromPrefab.Add(prefab, pool);
                _poolCount++;
            }
            obj = pool.Get();
            _poolsFromObject.TryAdd(obj, pool);

            return obj;
        }

        public IEnumerator Release(GameObject obj, float time) {
            yield return new WaitForSeconds(time);
            Release(obj);
        }

        public void Release(GameObject obj) {
            if (_poolsFromObject.TryGetValue(obj, out var pool)) {
                pool.Release(obj);
            } else {
                Debug.LogError($"Tried to release unknown object '{obj.name}' into pool. Destroying instead", obj);
                Object.Destroy(obj);
            }
        }

        private void Awake() {
            _instance = this;
        }
    }
}
