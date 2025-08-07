using Places;
using System;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(
    name: "MoveToPlacePosition",
    story: "[Self] navigates to [Place] position",
    category: "_NPC",
    id: "2ce0003ee9d6a477e9a59f907b425940")
]
public partial class MoveToPlacePositionAction : Unity.Behavior.Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<BasePlace> Place;

    [SerializeReference] public BlackboardVariable<string> AnimatorSpeedParam = new BlackboardVariable<string>("SpeedMagnitude");
    [SerializeReference] public BlackboardVariable<float> DistanceThreshold = new BlackboardVariable<float>(0.2f);

    private NavMeshAgent _navMeshAgent;
    private Animator _animator;

    private float _currentSpeed;

    protected override Status OnStart()
    {
        _navMeshAgent = Self.Value.GetComponent<NavMeshAgent>();
        _animator = Self.Value.GetComponent<Animator>();

        if (!_navMeshAgent || !_navMeshAgent.isOnNavMesh) return Status.Failure;
        if (!Place.Value) return Status.Failure;

        return Status.Running;

    }

    protected override Status OnUpdate()
    {
        _navMeshAgent.SetDestination(Place.Value.Position);
        if (Vector2.Distance(Self.Value.transform.position, Place.Value.Position) < (0.5f * Place.Value.Neighbourhood))
        {
            return Status.Success;
        }
        return Status.Running;

    }

    protected override void OnEnd()
    {
    }

    private void UpdateAnimatorSpeed(float explicitSpeed = -1)
    {
        // NavigationUtility.UpdateAnimatorSpeed(_animator, AnimatorSpeedParam, _navMeshAgent, _currentSpeed, explicitSpeed: explicitSpeed);
    }
}
