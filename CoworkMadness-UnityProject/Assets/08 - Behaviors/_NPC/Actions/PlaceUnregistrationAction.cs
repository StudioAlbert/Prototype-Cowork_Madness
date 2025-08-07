using Places;
using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(
    name: "PlaceUnregistration",
    story: "[Self] unregisters from [Place]",
    category: "Action",
    id: "6f1fda6fd8d769617fd29c3eaed4bd22")]
public partial class PlaceUnregistrationAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<BasePlace> Place;

    protected override Status OnStart()
    {
        // If can not UNregister, pass trough anyway
        if (!Place.Value.UnregisterUser(Self.Value))
        {
            Debug.LogWarning($"Place {Place.Value.name} : Can not unregister {Self.Value.name}");
        }
        return Status.Success;
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}
