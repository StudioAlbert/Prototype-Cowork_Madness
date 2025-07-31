using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Places.Queue;


public class NpcQueuer : MonoBehaviour
{
    [SerializeField] private QueueManager _queueManager;
    [SerializeField] private QueueCandidate _candidate;
    
    private NavMeshAgent _agent;
    private Vector3 _startPosition;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _candidate = GetComponent<QueueCandidate>();
        _startPosition = transform.position;
    }

    private void OnEnable() => _queueManager.Register(_candidate);
    private void OnDisable()
    {
        if (_queueManager.Unregister(_candidate))
        {
            _agent.destination = _startPosition;
        }
    }

    private void Update()
    {
        if (_candidate.QueuePoint && _agent.destination != _candidate.QueuePoint.transform.position)
        {
            _agent.destination = _candidate.QueuePoint.transform.position;
            Debug.Log("Changing destination " + _agent.destination);
        }
        else
        {
            _agent.destination = _startPosition;
        }
    }
    
}
