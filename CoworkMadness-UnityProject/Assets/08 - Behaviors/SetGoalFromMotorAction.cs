using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "set Goal from Motor", story: "set [UpdatedGoal] from [Motor]", category: "_NPC", id: "4634defad18016a8310a966b86f4d847")]
public partial class SetGoalFromMotorAction : Action
{
    [SerializeReference] public BlackboardVariable<GoalType> UpdatedGoal;
    [SerializeReference] public BlackboardVariable<GoalMotor> Motor;

    private GoalMotor _motor;
    
    protected override Status OnStart()
    {
        _motor = Motor.Value;
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        UpdatedGoal.Value = _motor.BestGoalType;
        Debug.Log($"Get Goal : {UpdatedGoal.Value} !");
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

