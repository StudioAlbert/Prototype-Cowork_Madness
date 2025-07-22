using System;
using Places;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(
    name: "UnregisterFromPlace", 
    story: "[Self] : Unregister from [BestPlace]", 
    category: "_NPC", 
    id: "9baeda7e6d059da25f023f93ef4fb26e")]
public partial class UnregisterFromPlaceAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<SimplePlace> BestPlace;
    
    private SimplePlace _place;

    protected override Status OnStart()
    {
        BestPlace.Value.UnregisterUser(Self);
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

