using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

using AI_Motivation;

[Serializable, GeneratePropertyBag]
[NodeDescription(
    name: "Set Goal from Motor", 
    story: "[Self] : Set GoalType [UpdatedGoal]", 
    category: "_NPC", 
    id: "4634defad18016a8310a966b86f4d847")]
public partial class SetGoalFromMotorAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<GoalType> UpdatedGoal;

    private GoalMotor _motor;
    
    protected override Status OnStart()
    {
        _motor = Self.Value.GetComponent<GoalMotor>();
        UpdatedGoal.Value = _motor ? _motor.BestGoalType : GoalType.Idle;
        
        Debug.Log($"Get Goal : {UpdatedGoal.Value} !");

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

