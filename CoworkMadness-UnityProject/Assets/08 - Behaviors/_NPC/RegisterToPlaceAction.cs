using Places;
using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(
    name: "RegisterToPlace", 
    story: "[Self] : Register to [BestPlace]", 
    category: "_NPC", 
    description: "if can register, SUCCESS. else FAILURE",
    id: "b034f9037937771d5b4439e92a6c2fa9")
]
public partial class RegisterToPlaceAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<SimplePlace> BestPlace;
    
    protected override Status OnStart()
    {
        if (!BestPlace.Value.Available)
            return Status.Failure;
        else
        {
            BestPlace.Value.RegisterUser(Self.Value);
            return Status.Running;
        }
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

