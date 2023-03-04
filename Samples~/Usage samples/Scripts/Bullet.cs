using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour {
    [SerializeField, HideInInspector] Rigidbody _rigidbody;

    void OnEnable() {
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
    }

    public void Fire(float launchForce) {
        _rigidbody.AddForce(transform.forward * launchForce, ForceMode.VelocityChange);
    }

    // Editor
    void OnValidate() {
        _rigidbody = GetComponent<Rigidbody>();
        Assert.IsNotNull(_rigidbody);
    }
}
