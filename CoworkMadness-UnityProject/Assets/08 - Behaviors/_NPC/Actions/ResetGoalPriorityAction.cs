using System;
using AI_Motivation;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
// ReSharper disable InconsistentNaming

[Serializable, GeneratePropertyBag]
[NodeDescription(
    name: "ResetGoalPriority", 
    story: "[Self] : Set [GoalType] 's priority to [NewPriorityValue]", 
    icon: "Icons/sequence",
    category: "_NPC", 
    id: "ca05f0d074eeded0664d9ba725142e0c")
]
public partial class ResetGoalPriorityAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<GoalType> GoalType;
    [SerializeReference] public BlackboardVariable<float> NewPriorityValue;

    private GoalMotor _motor;
    
    protected override Status OnStart()
    {
        _motor = Self.Value.GetComponent<GoalMotor>();
        _motor?.ResetPriority(GoalType.Value, NewPriorityValue.Value);
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

