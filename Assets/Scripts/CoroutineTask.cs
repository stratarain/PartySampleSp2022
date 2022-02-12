using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class CoroutineTask {

	public static readonly YieldInstruction WaitForNextFrame = new YieldInstruction();

	public readonly MonoBehaviour monoBehaviour;
	public CoroutineTaskType coroutineTaskType = CoroutineTaskType.Overwrite;

	public bool HasCoroutine => _coroutine != null;
	
	private Coroutine _coroutine;

	public CoroutineTask(MonoBehaviour monoBehaviour, CoroutineTaskType coroutineTaskType = CoroutineTaskType.Overwrite) {
		this.monoBehaviour = monoBehaviour;
		this.coroutineTaskType = coroutineTaskType;
	}

	public bool StartCoroutine(IEnumerator routine) {
		if (!monoBehaviour) {
			_coroutine = null;
			return false;
		}
		
		if (_coroutine == null) {
			_coroutine = monoBehaviour.StartCoroutine(routine);
			return true;
		}
		
		switch (coroutineTaskType) {
			case CoroutineTaskType.Overwrite: {
				monoBehaviour.StopCoroutine(_coroutine);
				_coroutine = monoBehaviour.StartCoroutine(routine);
				return true;
			}
				
			case CoroutineTaskType.Replace: {
				_coroutine = monoBehaviour.StartCoroutine(routine);
				return true;
			}
				
			case CoroutineTaskType.Ignore: return false;
		}

		return false;
	}

	public void StopCoroutine() {
		if (monoBehaviour && HasCoroutine) monoBehaviour.StopCoroutine(_coroutine);
		_coroutine = null;
	}
	
	public static IEnumerator TriggerEventDelayed(WaitForSeconds wait, UnityEvent toTrigger) {
		yield return wait;
		toTrigger?.Invoke();
	}
}

public enum CoroutineTaskType {
	Overwrite, // Stop the exist coroutine and start the current coroutine
	Replace, // Replace the exist coroutine with the current coroutine
	Ignore // Keep the exist coroutine and ignore the current coroutine
}