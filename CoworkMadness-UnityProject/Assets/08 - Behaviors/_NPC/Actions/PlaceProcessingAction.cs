using System;
using Unity.Behavior;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine;
using UnityEngine.AI;
using Places;
using Places.Queue;

[Serializable, GeneratePropertyBag]
[NodeDescription(
    name: "PlaceProceesing",
    story: "[Self] processes the [Place]",
    category: "Action",
    id: "a6c5ba5da73732b61febe22925d55f85")
]
public partial class PlaceProcessingAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<BasePlace> Place;
    [SerializeReference] public BlackboardVariable<float> PointDistance;

    private QueueCandidate _candidate;
    private QueueManager _manager;
    private NavMeshAgent _navMeshAgent;

    protected override Status OnStart()
    {
        // Get components and fail if unvalids
        _candidate = Self.Value.GetComponent<QueueCandidate>();
        _manager = Place.Value.GetComponent<QueueManager>();
        _navMeshAgent = Self.Value.GetComponent<NavMeshAgent>();

        if (!_manager || !_candidate || !_navMeshAgent || !_navMeshAgent.isOnNavMesh) return Status.Failure;

        _candidate.StartWait();
        
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        // Can abort
        if (Place.Value.CanAbort && !_manager.IsQueueDone(_candidate))
        {
            if(!_candidate.CanWait(Time.deltaTime))
            {
                Debug.Log($"{Self.Value.name} : Can not wait anymore at {Place.Value.name} !");
                return Status.Failure;
            }
        }
        
        // Go from the nearest queue point
        Vector3 queuePointPosition = _candidate.QueuePoint.transform.position;
        _navMeshAgent.SetDestination(queuePointPosition);

        if (Vector3.Distance(Self.Value.transform.position, queuePointPosition) <= PointDistance)
        {
            // Make the queue until queue is over (Last point)
            if (_manager.IsQueueDone(_candidate))
            {
                // Start process
                if (Place.Value.ProcessStatus == Places.Process.Status.Unset)
                {
                    Place.Value.StartProcess();
                }
                else
                {
                    Place.Value.Process(Time.deltaTime);
                    Debug.Log($"{Self.Value.name} : Process status [{Place.Value.ProcessStatus}], progress [{Place.Value.ProcessProgress:P0}]");

                    if (Place.Value.ProcessStatus == Places.Process.Status.Failed)
                        return Status.Failure;
                    if (Place.Value.ProcessStatus == Places.Process.Status.Success)
                        return Status.Success;

                }
            }
        }
        return Status.Running;


        // Place is processing


    }

    protected override void OnEnd()
    {
        Place.Value.StopProcess();
    }
}
