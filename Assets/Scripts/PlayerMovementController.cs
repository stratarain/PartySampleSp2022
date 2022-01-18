using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovementController : MonoBehaviour {

	public const string HORIZONTAL_MOVEMENT = "Horizontal";

	[Header("Setup")]
	public KeyCode forwardKey = KeyCode.W;
	public KeyCode backwardKey = KeyCode.S;
	public KeyCode rightKey = KeyCode.A;
	public KeyCode leftKey = KeyCode.D;
	public KeyCode runKey = KeyCode.LeftShift;
	public float walkSpeed = 5f;
	public float runSpeed = 10f;
	public bool doesDamp;
	public float damp = 1f;
	public Transform cameraRoot;

	public Action inControlEnter;
	public Action inControlExit;
	public Action onRunEnter;
	public Action onRunExit;

	public bool InControl {
		get => _inControl;
		set {
			_inControl = value;
			if (_inControl) inControlEnter?.Invoke();
			else inControlExit?.Invoke();
		}
	}

	[Header("Runtime (Don't Change)")]
	private bool _inControl = true;
	[SerializeField]
	private bool _needsUpdate;
	[SerializeField]
	private bool _running;
	[SerializeField]
	private float _forwardInput;
	[SerializeField]
	private float _rightInput;
	[SerializeField]
	private Vector3 _velocity;
	[SerializeField]
	private Collider _bodyCollider;
	[SerializeField]
	private Rigidbody _rigidbody;

	private void OnDrawGizmos() {
		Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
		Gizmos.color = Color.green;
		Gizmos.DrawCube(Vector3.zero, Vector3.one);
		Vector3 right = Vector3.right * 2f;
		Vector3 left = Vector3.left * 2f;
		Vector3 forward = Vector3.forward * 2f;
		Gizmos.DrawLine(Vector3.zero, Vector3.Slerp(right, forward, .25f));
		Gizmos.DrawLine(Vector3.zero, Vector3.Slerp(right, forward, .375f));
		Gizmos.DrawLine(Vector3.zero, Vector3.Slerp(right, forward, .5f));
		Gizmos.DrawLine(Vector3.zero, Vector3.Slerp(right, forward, .625f));
		Gizmos.DrawLine(Vector3.zero, Vector3.Slerp(right, forward, .75f));
		Gizmos.DrawLine(Vector3.zero, Vector3.Slerp(right, forward, .875f));
		Gizmos.DrawLine(Vector3.zero, Vector3.Slerp(left, forward, .25f));
		Gizmos.DrawLine(Vector3.zero, Vector3.Slerp(left, forward, .375f));
		Gizmos.DrawLine(Vector3.zero, Vector3.Slerp(left, forward, .5f));
		Gizmos.DrawLine(Vector3.zero, Vector3.Slerp(left, forward, .625f));
		Gizmos.DrawLine(Vector3.zero, Vector3.Slerp(left, forward, .75f));
		Gizmos.DrawLine(Vector3.zero, Vector3.Slerp(left, forward, .875f));
		Gizmos.DrawLine(Vector3.zero, forward);
	}
	
	private void OnDrawGizmosSelected() {
		Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
		Gizmos.color = Color.green;
		Gizmos.DrawCube(Vector3.zero, Vector3.one);
		Gizmos.color = new Color(1f, 0.38f, 0.04f);
		Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
		Vector3 right = Vector3.right * 2f;
		Vector3 left = Vector3.left * 2f;
		Vector3 forward = Vector3.forward * 2f;
		Gizmos.DrawLine(Vector3.zero, Vector3.Slerp(right, forward, .25f));
		Gizmos.DrawLine(Vector3.zero, Vector3.Slerp(right, forward, .375f));
		Gizmos.DrawLine(Vector3.zero, Vector3.Slerp(right, forward, .5f));
		Gizmos.DrawLine(Vector3.zero, Vector3.Slerp(right, forward, .625f));
		Gizmos.DrawLine(Vector3.zero, Vector3.Slerp(right, forward, .75f));
		Gizmos.DrawLine(Vector3.zero, Vector3.Slerp(right, forward, .875f));
		Gizmos.DrawLine(Vector3.zero, Vector3.Slerp(left, forward, .25f));
		Gizmos.DrawLine(Vector3.zero, Vector3.Slerp(left, forward, .375f));
		Gizmos.DrawLine(Vector3.zero, Vector3.Slerp(left, forward, .5f));
		Gizmos.DrawLine(Vector3.zero, Vector3.Slerp(left, forward, .625f));
		Gizmos.DrawLine(Vector3.zero, Vector3.Slerp(left, forward, .75f));
		Gizmos.DrawLine(Vector3.zero, Vector3.Slerp(left, forward, .875f));
		Gizmos.DrawLine(Vector3.zero, forward);
	}

	private void Awake() {
		_bodyCollider = GetComponent<Collider>();
		_rigidbody = GetComponent<Rigidbody>();
	}

	private void FixedUpdate() => _needsUpdate = true;

	private void Update() {
		if (!_needsUpdate) return;
		_needsUpdate = false;
		UpdateInput();
		UpdateMovement();
	}

	private void UpdateInput() {
		float speed;
		
		if (Input.GetKey(runKey)) {
			speed = runSpeed;
			if (!_running) {
				_running = true;
				onRunEnter?.Invoke();
			}
		} else {
			speed = walkSpeed;
			if (_running) {
				_running = false;
				onRunExit?.Invoke();
			}
		}

		if (Input.GetKey(forwardKey)) _forwardInput = speed;
		else if (Input.GetKey(backwardKey)) _forwardInput = -speed;
		else _forwardInput = 0f;

		if (Input.GetKey(rightKey)) _rightInput = speed;
		else if (Input.GetKey(leftKey)) _rightInput = -speed;
		else _rightInput = 0f;
	}

	private void UpdateMovement() {
		_velocity = new Vector3(-_rightInput, 0f, _forwardInput);
		_velocity = transform.TransformVector(_velocity);
		_rigidbody.velocity = doesDamp ? Vector3.MoveTowards(_rigidbody.velocity, _velocity, damp * Time.deltaTime) : _velocity;
	}
}