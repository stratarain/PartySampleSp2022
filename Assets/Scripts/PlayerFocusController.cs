using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public class PlayerFocusController : MonoBehaviour {

	[Header("Setup")]
	public string xAxisName = "Mouse X";
	public string yAxisName = "Mouse Y";
	public bool locksCursor = true;
	public bool horizontalReverted;
	public bool verticalReverted = true;
	public bool horizontalLocked;
	public bool verticalLocked = true;
	public float horizontalSensitivity = 5f;
	public float verticalSensitivity = 5f;
	public Transform cameraRoot;
	public Camera mainCamera;
	public Camera weaponCamera;

	private void OnEnable() => CheckAndLockCursor();
	
	private void Update() {
		if (!horizontalLocked) {
			float hSen = horizontalSensitivity * (horizontalReverted ? -1f : 1f);
			transform.Rotate(0, Input.GetAxis(xAxisName) * hSen, 0);
			Vector3 euler = transform.localRotation.eulerAngles;
			euler.z = 0f;
			transform.localRotation = Quaternion.Euler(euler);
		} else {
			Vector3 euler = transform.localRotation.eulerAngles;
			euler.y = 0f;
			euler.z = 0f;
			transform.localRotation = Quaternion.Euler(euler);
		}
		
		if (!verticalLocked) {
			float vSen = verticalSensitivity * (verticalReverted ? -1f : 1f);
			cameraRoot.Rotate(new Vector3(Input.GetAxis(yAxisName) * vSen, 0, 0));
			Vector3 euler = cameraRoot.localRotation.eulerAngles;
			euler.z = 0f;
			cameraRoot.localRotation = Quaternion.Euler(euler);
		} else {
			Vector3 euler = transform.localRotation.eulerAngles;
			euler.x = 0f;
			euler.z = 0f;
			transform.localRotation = Quaternion.Euler(euler);
		}
	}

	private void OnApplicationFocus(bool hasFocus) {
		if (hasFocus) CheckAndLockCursor();
	}

	public void CheckAndLockCursor() {
		if (locksCursor) LockCursor();
	}

	public void LockCursor() => Cursor.lockState = CursorLockMode.Locked;
}