using UnityEngine;

public class VerticalBillboardController : MonoBehaviour {

    private int _scaleID;
    private Material _material;

    private void Awake() {
        _material = GetComponent<Renderer>().material;
        _scaleID = Shader.PropertyToID("_ScaleXY");
    }

    private void Update() {
        var scale = transform.lossyScale;
        _material.SetVector(_scaleID, scale);
    }
}