using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "TestAction", story: "[x] loops [message] before quit", category: "Test One Two", id: "74f53921f449e11fdbecab995a8102bb")]
public partial class TestAction : Action
{
    [SerializeReference] public BlackboardVariable<int> X;
    [SerializeReference] public BlackboardVariable<string> Message;

    private int _counter;
    
    protected override Status OnStart()
    {
        Debug.Log("Starting TestAction");
        _counter = 0;
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if(_counter < X.Value)
        {
            Debug.Log($"Counter {_counter} : {Message.Value}");
            _counter++;
            return Status.Running;
        }
        else
        {
            Debug.Log("TestAction finished : Success");
            return Status.Success;
        }
    }

    protected override void OnEnd()
    {
        Debug.Log("TestAction finished : Failure");
    }
}

