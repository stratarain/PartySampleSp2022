using System;
using UnityEngine;
using UnityEngine.AI;

public class NavAgentTest : MonoBehaviour {

    [Header("Setup")]
    public NavMeshAgent agent;
    public Transform target;
    public float updateInterval = 1.0f;
    public bool autoUpdatePosition = true;
    public bool autoUpdateRotation = true;

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
            var r = agent.SetDestination(transform.position);
            // agent.destination = transform.position;
            print("R: " + r);
        }
        
        print(agent.nextPosition.ToString("F3"));
    }

    private void OnValidate() {
        agent.updatePosition = autoUpdatePosition;
        agent.updateRotation = autoUpdateRotation;
    }
}