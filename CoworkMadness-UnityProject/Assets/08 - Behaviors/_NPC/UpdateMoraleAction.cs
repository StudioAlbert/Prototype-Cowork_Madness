using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.Serialization;
using UnityEngine.TextCore.Text;
using AI_Motivation;

[Serializable, GeneratePropertyBag]
[NodeDescription(
    name: "UpdateMorale", 
    story: "[_self] : Update morale from [_goalType] / [_successfulOrNot]", 
    category: "_NPC", 
    id: "b2eeb99250114e736e54f20f70fd9421")
]
public partial class UpdateMoraleAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<GoalType> GoalType;
    [SerializeReference] public BlackboardVariable<bool> SuccessfulOrNot;
    
    private Morale _morale;
    
    protected override Status OnStart()
    {
        _morale = Self.Value.GetComponent<Morale>();
        _morale?.UpdateMoraleAmongGoals(GoalType, SuccessfulOrNot);
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

