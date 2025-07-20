using System;
using Places;
using Unity.Behavior;
using UnityEngine;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(
    name: "Get from Place", 
    story: "from [Place] : [available]", 
    category: "_NPC", 
    id: "8955ec536775b11fca2ef1349de4613f")
]
public partial class GetFromPlaceModifier : Unity.Behavior.Action
{
    [SerializeReference] public BlackboardVariable<SimplePlace> Place;
    [SerializeReference] public BlackboardVariable<bool> Available;

    private SimplePlace _place;
    
    protected override Status OnStart()
    {
        _place = Place.Value;
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        Available.Value = _place.Available;
        Debug.Log($"Get from Place : {_place.Available} ! Get from Variable : {Available.Value} !");
        return Status.Success;
    }

    protected override void OnEnd()
    {
        
    }
}

