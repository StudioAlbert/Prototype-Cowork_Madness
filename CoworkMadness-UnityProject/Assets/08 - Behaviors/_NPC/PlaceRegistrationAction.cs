using Places;
using System;
using Places.Queue;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "PlaceRegistration",
    story: "[Self] registers to [Place]",
    category: "Action",
    id: "1f173d55b5cb39824b3ac4628e0ab4d0")]
public partial class PlaceRegistrationAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<BasePlace> Place;

    protected override Status OnStart()
    {
        if (!Place.Value.Available || !Place.Value.RegisterUser(Self.Value))
        {
            return Status.Failure;
        }
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
