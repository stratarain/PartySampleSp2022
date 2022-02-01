using System;
using UnityEngine;
using UnityEngine.AI;

public class NavAgentTest : MonoBehaviour {

    [Header("Setup")]
    public NavMeshAgent agent;
    public Transform target;
    public Vector3 offset;
    public float updateInterval = 1.0f;
    public bool autoUpdatePosition = true;
    public bool autoUpdateRotation;

    [Header("Runtime")]
    [SerializeField]
    private float _lastUpdate = -1f;

    private void Awake() {
        agent.updatePosition = autoUpdatePosition;
        agent.updateRotation = autoUpdateRotation;
    }

    private void Update() {
        if (Time.frameCount == 2) agent.enabled = true;
        
        if (!agent || !target || !agent.isOnNavMesh) return;

        var time = Time.time;
        if (_lastUpdate < .0f || time - _lastUpdate >= updateInterval) {
            _lastUpdate = time;
            var pos = target.position;
            agent.SetDestination(pos + offset);
        }
    }

    private void OnValidate() {
        agent.updatePosition = autoUpdatePosition;
        agent.updateRotation = autoUpdateRotation;
    }
}