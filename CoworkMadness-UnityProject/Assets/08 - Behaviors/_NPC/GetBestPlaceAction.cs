using Places;
using System;
using AI_Motivation;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "GetBestPlace", story: "[Self] , [PlaceProvider] : Get best place [BestPlace] for [GoalType]", category: "_NPC", id: "0aeda56294ee54ca21a7d5bd13177445")]
public partial class GetBestPlaceAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<PlaceProvider> PlaceProvider;
    [SerializeReference] public BlackboardVariable<BasePlace> BestPlace;
    [SerializeReference] public BlackboardVariable<GoalType> GoalType;

    private PlaceProvider _placeProvider;
    
    protected override Status OnStart()
    {
        _placeProvider = PlaceProvider.Value;
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        BasePlace place = _placeProvider.GetBestPlaceOfType(Self.Value.transform.position, GoalType.Value);
        if(!place) return Status.Running;
        
        BestPlace.Value = place;
        return Status.Success;
        
    }

    protected override void OnEnd()
    {
    }
}

