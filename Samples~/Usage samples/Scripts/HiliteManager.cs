using Lotec.Utils;
using UnityEngine;
using UnityEngine.Assertions;

public class HiliteManager : MonoBehaviour {
    [Header("Toggle hilite with left mouse button")]
    [SerializeField] GameObject _objectToHilite;

    bool _on;

    void Awake() {
        Hiliter.InitHilite();
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            ToggleHilite();
        }
    }

    void ToggleHilite() {
        _on = !_on;
        Hiliter.Hilite(_objectToHilite, _on);
    }

    void OnValidate() {
        Assert.IsNotNull(_objectToHilite, "Specify object to hilite");
    }
}
