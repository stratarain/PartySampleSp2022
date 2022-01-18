using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public sealed class DoorController : MonoBehaviour {

	public enum DoorOperationType {
		None,
		Open,
		Close
	}

	[Header("Setup")]
	public Transform door;
	public int openTimes = -1;
	public float transitionTime = .5f;

	[Header("Runtime")]
	[SerializeField]
	private bool _opened;
	[SerializeField]
	private int _openedTimes;
	[SerializeField]
	private DoorOperationType _currentDoorOperation;
	[SerializeField]
	private Vector3 _openShift;
	[SerializeField]
	private Vector3 _openPosition;
	[SerializeField]
	private Vector3 _closePosition;
	[SerializeField]
	private CoroutineTask _operationTask;

	private void Awake() {
		_operationTask = new CoroutineTask(this);
		_closePosition = door.localPosition;
	}

	public void Operate() {
		if (_opened) Close();
		else Open();
	}

	public void Open() {
		if (openTimes > 0 && _openedTimes >= openTimes) return;
		if (_opened && _currentDoorOperation == DoorOperationType.Close || !_opened && _currentDoorOperation != DoorOperationType.Open) {
			openTimes++;
			_currentDoorOperation = DoorOperationType.Open;
			_openPosition = _closePosition + _openShift;
			_operationTask.StartCoroutine(ExeOperationTask(_closePosition, _openPosition, true));
		}
	}

	public void Close() {
		if (!_opened && _currentDoorOperation == DoorOperationType.Open || _opened && _currentDoorOperation != DoorOperationType.Close) {
			_currentDoorOperation = DoorOperationType.Close;
			_openPosition = _closePosition + _openShift;
			_operationTask.StartCoroutine(ExeOperationTask(_openPosition, _closePosition, false));
		}
	}

	private IEnumerator ExeOperationTask(Vector3 initialPosition, Vector3 targetPosition, bool opens) {
		float progress = Mathf.Clamp01((door.localPosition.x - initialPosition.x) / (targetPosition.x - initialPosition.x));		
		float initialTime = Time.time - transitionTime * progress;
		while (progress < 1f) {
			yield return null;
			progress = Mathf.Clamp01((Time.time - initialTime) / transitionTime);
			door.localPosition = Vector3.Lerp(initialPosition, targetPosition, progress);
		}

		door.localPosition = targetPosition;
		_opened = opens;
		_currentDoorOperation = DoorOperationType.None;
		_operationTask.StopCoroutine();
	}

	private void OnTriggerEnter() => Open();

	private void OnTriggerExit() => Close();
}