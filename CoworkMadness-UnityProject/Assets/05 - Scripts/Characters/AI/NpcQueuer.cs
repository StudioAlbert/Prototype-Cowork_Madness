using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Places.Queue;


public class NpcQueuer : QueueCandidate
{
    [SerializeField] private QueueManager _queueManager;
    private NavMeshAgent _agent;
    private Vector3 _startPosition;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _startPosition = transform.position;
    }

    private void OnEnable() => Register();
    private void OnDisable() => Unregister();

    private void Update()
    {
        if (QueuePoint && _agent.destination != QueuePoint.transform.position)
        {
            _agent.destination = QueuePoint.transform.position;
            Debug.Log("Changing destination " + _agent.destination);
        }
        else
        {
            _agent.destination = _startPosition;
        }
    }

    // Queue Candidates Overrides
    protected override QueueManager QueueManager
    {
        get => _queueManager;
        set => _queueManager = value;
    }

    public override void Register()
    {
        if (QueueManager.Register(this, out QueuePoint qp))
        {
            QueuePoint = qp;
        }
        else
        {
            QueuePoint = null;
        }
    }
    public override void Unregister()
    {
        if (QueueManager.Unregister(this))
        {
            _agent.destination = _startPosition;
        }
    }
}
