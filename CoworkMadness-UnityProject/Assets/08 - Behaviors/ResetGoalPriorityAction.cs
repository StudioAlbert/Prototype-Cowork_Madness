using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
// ReSharper disable InconsistentNaming

[Serializable, GeneratePropertyBag]
[NodeDescription(
    name: "ResetGoalPriority", 
    story: "Reset [GoalType] in [Motor] to [NewPriorityValue]", 
    icon: "Icons/sequence",
    category: "_NPC", 
    id: "ca05f0d074eeded0664d9ba725142e0c")
]
public partial class ResetGoalPriorityAction : Action
{
    [SerializeReference] public BlackboardVariable<GoalType> GoalType;
    [SerializeReference] public BlackboardVariable<GoalMotor> Motor;
    [SerializeReference] public BlackboardVariable<float> NewPriorityValue;

    private GoalMotor _motor;
    
    protected override Status OnStart()
    {
        _motor = Motor.Value;
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        _motor.ResetPriority(GoalType.Value, NewPriorityValue.Value);
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

