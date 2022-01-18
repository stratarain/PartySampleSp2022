using UnityEngine;

public class CameraWanderer : MonoBehaviour {

	[Header("Setup")]
	public KeyCode forwardKey = KeyCode.W;
	public KeyCode backwardKey = KeyCode.S;
	public KeyCode leftKey = KeyCode.A;
	public KeyCode rightKey = KeyCode.D;
	public KeyCode accelerationKey = KeyCode.LeftShift;
	public KeyCode decelerationKey = KeyCode.C;
	public int dragMouseButton;
	public float normalSpeed = 5f;
	public float accelerationSpeed = 20f;
	public float decelerationSpeed = 1f;
	public float normalSensitivity = 5f;
	public float accelerationSensitivity = 10f;
	public float decelerationSensitivity = 1f;

	private void Update() {
		float speed;
		float sensitivity;
		if (Input.GetKey(accelerationKey)) {
			speed = accelerationSpeed;
			sensitivity = accelerationSensitivity;
		} else if (Input.GetKey(decelerationKey)) {
			speed = decelerationSpeed;
			sensitivity = decelerationSensitivity;
		} else {
			speed = normalSpeed;
			sensitivity = normalSensitivity;
		}

		if (dragMouseButton != -1 && Input.GetMouseButton(dragMouseButton)) {
			transform.Rotate(new Vector3(-Input.GetAxis("Mouse Y") * sensitivity, Input.GetAxis("Mouse X") * sensitivity, 0));
			Vector3 euler = transform.rotation.eulerAngles;
			euler.z = 0f;
			transform.rotation = Quaternion.Euler(euler);
		}

		float movement = speed * Time.deltaTime;

		if (Input.GetKey(forwardKey)) transform.position += transform.forward * movement;
		else if (Input.GetKey(backwardKey)) transform.position -= transform.forward * movement;

		if (Input.GetKey(rightKey)) transform.position += transform.right * movement;
		else if (Input.GetKey(leftKey)) transform.position -= transform.right * movement;
	}
}