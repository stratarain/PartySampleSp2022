using UnityEngine;

public static class Utilities {

	public static Vector3 GetLocalPosition(Matrix4x4 transform) => transform.GetColumn(3);
	public static Quaternion GetLocalRotation(Matrix4x4 transform) => Quaternion.LookRotation(transform.GetColumn(2), transform.GetColumn(1));
	public static Vector3 GetLocalScale(Matrix4x4 transform) => new Vector3(transform.GetColumn(0).magnitude, transform.GetColumn(1).magnitude, transform.GetColumn(2).magnitude);

	public static (Vector3, Quaternion, Vector3) GetLocalTransform(Matrix4x4 transform) {
		Vector4 col1 = transform.GetColumn(1);
		Vector4 col2 = transform.GetColumn(2);
		Vector3 position = transform.GetColumn(3);
		Quaternion rotation = Quaternion.LookRotation(col2, col1);
		Vector3 scale = new Vector3(transform.GetColumn(0).magnitude, col1.magnitude, col2.magnitude);
		return (position, rotation, scale);
	}
}