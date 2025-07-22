using System;
using System.Collections.Generic;
using System.Linq;
using AI;
using GOAP;
using Places;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Utilities;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Inventory))]
// ReSharper disable once CheckNamespace
public class NpcAgent : GoapAgent
{
    private const float KTargetDistance = 0.5f;
    private Rigidbody _rb;
    private Animator _animator;
    private Inventory _inventory;

    // TODO Place provider
    [FormerlySerializedAs("desk")]
    [Header("Places")]
    [SerializeField] private SimplePlace _desk;
    [SerializeField] private SimplePlace _coffeeMachineA;
    [SerializeField] private SimplePlace _coffeeMachineB;
    [SerializeField] private BasePlace _terrace;
    [SerializeField] private Transform _talkPerson;

    private void Awake()
    {
        _navMesh = GetComponent<NavMeshAgent>();
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        _inventory = GetComponent<Inventory>();
        _logger = GetComponent<LoggerObject>();

        _planner = new GoapPlanner(_logger);

    }

    protected override void SetupBeliefs()
    {
        _beliefs = new Dictionary<string, GoapBelief>();
        GoapBeliefFactory bFactory = new GoapBeliefFactory(this, _beliefs);

        bFactory.AddBelief("Nothing", () => false);
        bFactory.AddBelief("AgentIdle", () => !_navMesh.hasPath);
        bFactory.AddBelief("AgentMoving", () => _navMesh.hasPath);

        bFactory.AddBelief("HadABreak", () => false);
        BuildCoffeeMachineBeliefs(_coffeeMachineA, "_A");
        BuildCoffeeMachineBeliefs(_coffeeMachineB, "_B");
        bFactory.AddBelief("HasACoffee", () => _inventory.CoffeeEquipped);
        bFactory.AddLocationBelief("AtTheTerrace", KTargetDistance, _terrace.Position);

        bFactory.AddBelief("HasDesk", () => _desk.Available);
        bFactory.AddLocationBelief("AtDesk", KTargetDistance, _desk.Position);
        bFactory.AddBelief("MakeMoney", () => false);

        bFactory.AddBelief("HadATalk", () => false);
        bFactory.AddLocationBelief("MetSomeone", KTargetDistance, _talkPerson);
        
    }

    protected override void SetupActions()
    {
        _actions = new List<GoapAction>();

        _actions.Add(new GoapAction.Builder("Nothing")
            .WithStrategy(new IdleStrategy(0))
            .AddPostCondition(_beliefs["Nothing"])
            .Build());

        BuildCoffeeMachineActions(_coffeeMachineA, "_A");
        BuildCoffeeMachineActions(_coffeeMachineB, "_B");

        _actions.Add(new GoapAction.Builder("GoToTheTerrace")
            .WithStrategy(new MoveStrategy(_navMesh, KTargetDistance, () => _terrace.Position))
            .AddPrecondition(_beliefs["HasACoffee"])
            .AddPostCondition(_beliefs["AtTheTerrace"])
            .Build());
        _actions.Add(new GoapAction.Builder("DrinkCoffee")
            .WithStrategy(new IdleStrategy(6))
            .AddPrecondition(_beliefs["AtTheTerrace"])
            .AddPostCondition(_beliefs["HadABreak"])
            .AddConsequence(() => _inventory.CoffeeEquipped = false)
            .Build());


        _actions.Add(new GoapAction.Builder("GoToDesk")
            .WithStrategy(new MoveStrategy(_navMesh, KTargetDistance, () => _desk.Position))
            //.AddPrecondition(_beliefs["HasDesk"])
            .AddPostCondition(_beliefs["AtDesk"])
            .Build());
        // _actions.Add(new GoapAction.Builder("WaitForDesk")
        //     .WithStrategy(new QueueStrategy(5, desk))
        //     //.AddPrecondition(_beliefs["HasDesk"])
        //     .AddPostCondition(_beliefs["HasDesk"])
        //     .Build());
        _actions.Add(new GoapAction.Builder("Work")
            .WithStrategy(new UsingMachineStrategy(3, _desk, gameObject))
            .AddPrecondition(_beliefs["AtDesk"])
            .AddPostCondition(_beliefs["MakeMoney"])
            .Build());

        _actions.Add(new GoapAction.Builder("SmallTalkToBoss")
            .WithStrategy(new MoveStrategy(_navMesh, KTargetDistance, () => _talkPerson.position))
            .AddPostCondition(_beliefs["MetSomeone"])
            .Build());
        // _actions.Add(new GoapAction.Builder("SmallTalkToBoss")
        //     .WithStrategy(new IdleStrategy(3))
        //     .AddPrecondition(_beliefs["MetSomeone"])
        //     .AddPostCondition(_beliefs["HadATalk"])
        //     .Build());

    }
    protected override void SetupGoals()
    {
        _goals = new List<GoapGoal>();

        _goals.Add(new GoapGoal.Builder("Nothing")
            .WithPriority(0.01f)
            .WithType(Places.PlaceType.None)
            .WithDesiredEffect(_beliefs["Nothing"])
            .Build());

        _goals.Add(new GoapGoal.Builder("HaveABreak")
            .WithPriority(0.1f)
            .WithType(Places.PlaceType.Break)
            .WithDesiredEffect(_beliefs["HadABreak"])
            .Build());

        _goals.Add(new GoapGoal.Builder("MakeMoney")
            .WithPriority(1)
            .WithType(Places.PlaceType.Work)
            .WithDesiredEffect(_beliefs["MakeMoney"])
            .Build());
        
        _goals.Add(new GoapGoal.Builder("Talk")
            .WithPriority(0.1f)
            .WithType(Places.PlaceType.Social)
            .WithDesiredEffect(_beliefs["HadATalk"])
            .Build());

    }
    private void BuildCoffeeMachineBeliefs(SimplePlace coffeeMachine, string suffix)
    {
        GoapBeliefFactory bFactory = new GoapBeliefFactory(this, _beliefs);
        bFactory.AddLocationBelief("AtCoffeeMachine" + suffix, KTargetDistance, coffeeMachine.Position);
        bFactory.AddBelief("CoffeeMachineToMe" + suffix, () => coffeeMachine.User == gameObject);
        bFactory.AddBelief("CoffeeMachineAvailable" + suffix, () => coffeeMachine.Available);
    }
    private void BuildCoffeeMachineActions(SimplePlace coffeeMachine, string suffix)
    {
        _actions.Add(new GoapAction.Builder("GetACoffee"+suffix)
            .WithStrategy(new MoveStrategy(_navMesh, KTargetDistance, () => coffeeMachine.Position))
            .AddPostCondition(_beliefs["AtCoffeeMachine"+suffix])
            .Build());
        _actions.Add(new GoapAction.Builder("CheckMachine"+suffix)
            .WithStrategy(new IdleStrategy(0.1f))
            .AddPrecondition(_beliefs["AtCoffeeMachine"+suffix])
            .AddPostCondition(_beliefs["CoffeeMachineAvailable" + suffix])
            .Build());
        _actions.Add(new GoapAction.Builder("WaitForCoffeeMachine"+suffix)
            .WithStrategy(new QueueStrategy(20, coffeeMachine, gameObject))
            .AddPrecondition(_beliefs["CoffeeMachineAvailable" + suffix])
            .AddPostCondition(_beliefs["CoffeeMachineToMe" + suffix])
            .Build());
        _actions.Add(new GoapAction.Builder("MakeACoffee"+suffix)
            .WithStrategy(new UsingMachineStrategy(10, coffeeMachine, gameObject))
            .AddPrecondition(_beliefs["CoffeeMachineToMe"+suffix])
            .AddPostCondition(_beliefs["HasACoffee"])
            .AddConsequence(() => _inventory.CoffeeEquipped = true)
            .Build());
    }

}
