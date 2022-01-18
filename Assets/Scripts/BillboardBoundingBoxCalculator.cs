using UnityEngine;

public sealed class BillboardBoundingBoxCalculator : MonoBehaviour {

	private MeshFilter _meshFilter;
	private Renderer _renderer;
	
	private void Awake() {
		_meshFilter = GetComponent<MeshFilter>();
		_renderer = GetComponent<Renderer>();
		InitBoundingBox();
	}

	private void InitBoundingBox() {
		Mesh mesh = _meshFilter.mesh;
		Bounds bounds = _renderer.bounds;
		Vector3 extents = bounds.extents;
		float maxLength = Mathf.Sqrt(extents.x * extents.x + extents.z * extents.z);
		extents.x = maxLength;
		extents.z = maxLength;
		bounds.extents = transform.InverseTransformVector(extents);
		bounds.center = Vector3.zero;
		mesh.bounds = bounds;
	}
}