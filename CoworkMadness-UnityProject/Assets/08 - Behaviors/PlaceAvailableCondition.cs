using Places;
using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(
    name: "PlaceAvailable",
    story: "Is [Place] available equal to [AvailableValue]",
    category: "_NPC",
    id: "675530786a2150d02c20a56d6f780bd2")
]
public partial class PlaceAvailableCondition : Condition
{
    [SerializeReference] public BlackboardVariable<SimplePlace> Place;
    [SerializeReference] public BlackboardVariable<bool> AvailableValue;

    private SimplePlace _place;
    
    public override bool IsTrue()
    {
        return _place.Available == AvailableValue.Value;
    }

    public override void OnStart()
    {
        _place = Place.Value;
    }

    public override void OnEnd()
    {
    }
}
