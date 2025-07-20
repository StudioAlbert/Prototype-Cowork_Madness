using System;
using Unity.Behavior;
using UnityEngine;
using Unity.Properties;

#if UNITY_EDITOR
[CreateAssetMenu(menuName = "Behavior/Event Channels/GoalEnds")]
#endif
[Serializable, GeneratePropertyBag]
[EventChannelDescription(name: "GoalEnds", message: "[Goal] achieved [Success]", category: "_NPC", id: "67bff2d0a055628f4cd2a6cc278d9c6b")]
public sealed partial class GoalEnds : EventChannel<GoalType, bool> { }

