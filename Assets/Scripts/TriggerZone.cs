using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class TriggerZone : MonoBehaviour {

    public bool playerOnly = true;
    public LayerMask mask;
    public float enterEventDelay;
    public UnityEvent enterEvent;
    public float exitEventDelay;
    public UnityEvent exitEvent;

    private WaitForSeconds _waitToEnter;
    private WaitForSeconds _waitToExit;

    private void Awake() {
        _waitToEnter = new WaitForSeconds(enterEventDelay);
        _waitToExit = new WaitForSeconds(exitEventDelay);
    }

    private void OnTriggerEnter(Collider other) {
        int layer = 1 << other.gameObject.layer;
        if ((mask.value & layer) != 0) {
            if (!playerOnly || other.CompareTag("Player")) {
                if (enterEventDelay == .0f) enterEvent?.Invoke();
                else StartCoroutine(CoroutineTask.TriggerEventDelayed(_waitToEnter, enterEvent));
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        int layer = 1 << other.gameObject.layer;
        if ((mask.value & layer) != 0) {
            if (!playerOnly || other.CompareTag("Player")) {
                if (exitEventDelay == .0f) exitEvent?.Invoke();
                else StartCoroutine(CoroutineTask.TriggerEventDelayed(_waitToExit, exitEvent));
            }
        }
    }

    private void OnDestroy() {
        StopAllCoroutines();
    }
}