using System;
using System.Linq;
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

    private GoalMotor _goalMotor;
    
    protected override Status OnStart()
    {
        _goalMotor = Self.Value.GetComponent<GoalMotor>();
        UpdatedGoal.Value = _goalMotor ? _goalMotor.GetBestGoalType() : GoalType.Idle;
        
        // string dbgMoods = "";
        // foreach (var m in _goalMotor.Moods.OrderByDescending(m => m.Priority))
        // {
        //     dbgMoods += $"{m.Type} : {m.Priority}\n";
        // }
        // Debug.Log($"Get Goal : {UpdatedGoal.Value}\nMoods : \n{dbgMoods}");
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

