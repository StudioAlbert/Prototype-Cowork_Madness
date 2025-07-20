using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.Serialization;
using UnityEngine.TextCore.Text;

[Serializable, GeneratePropertyBag]
[NodeDescription(
    name: "UpdateMorale", 
    story: "[_self] : update morale from [_goalType] / [_successfulOrNot]", 
    category: "Action", 
    id: "b2eeb99250114e736e54f20f70fd9421")
]
public partial class UpdateMoraleAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> _self;
    [SerializeReference] public BlackboardVariable<GoalType> _goalType;
    [SerializeReference] public BlackboardVariable<bool> _successfulOrNot;
    
    private Morale _morale;
    
    protected override Status OnStart()
    {
        _morale = _self.Value.GetComponent<Morale>();
        _morale.UpdateMoraleAmongGoals(_goalType, _successfulOrNot);
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

