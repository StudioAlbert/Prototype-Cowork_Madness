﻿using System;
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
    [FormerlySerializedAs("_desk")]
    [Header("Places")]
    [SerializeField] private SimplePlace desk;
    [SerializeField] private SimplePlace coffeeMachine;
    [SerializeField] private BasePlace terrace;
    [SerializeField] private Transform talkPerson;

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
        // TODO
        // Make a better (inventory/item class) prop/NPC system with feedback => have a coffee mug in the hand
        bFactory.AddLocationBelief("AtCoffeeMachine", KTargetDistance, coffeeMachine.Position);
        bFactory.AddBelief("CoffeeMachineAvailable", () => coffeeMachine.Available);
        bFactory.AddBelief("HasACoffee", () => _inventory.CoffeeEquipped);
        bFactory.AddLocationBelief("AtTheTerrace", KTargetDistance, terrace.Position);

        bFactory.AddBelief("HasDesk", () => desk.Available);
        bFactory.AddLocationBelief("AtDesk", KTargetDistance, desk.Position);
        bFactory.AddBelief("MakeMoney", () => false);

        bFactory.AddBelief("HadATalk", () => false);
        bFactory.AddLocationBelief("MetSomeone", KTargetDistance, talkPerson);
        
    }
    protected override void SetupActions()
    {
        _actions = new List<GoapAction>();

        _actions.Add(new GoapAction.Builder("Nothing")
            .WithStrategy(new IdleStrategy(0))
            .AddPostCondition(_beliefs["Nothing"])
            .Build());

        _actions.Add(new GoapAction.Builder("GetACoffee")
            .WithStrategy(new MoveStrategy(_navMesh, KTargetDistance, () => coffeeMachine.Position))
            .AddPostCondition(_beliefs["AtCoffeeMachine"])
            .Build());
        _actions.Add(new GoapAction.Builder("WaitForCoffeeMachine")
            .WithStrategy(new QueueStrategy(20, coffeeMachine))
            .AddPrecondition(_beliefs["AtCoffeeMachine"])
            .AddPostCondition(_beliefs["CoffeeMachineAvailable"])
            .Build());
        _actions.Add(new GoapAction.Builder("MakeACoffee")
            .WithStrategy(new UsingMachineStrategy(10, coffeeMachine, gameObject))
            .AddPrecondition(_beliefs["CoffeeMachineAvailable"])
            .AddPostCondition(_beliefs["HasACoffee"])
            .AddConsequence(() => _inventory.CoffeeEquipped = true)
            .Build());
        _actions.Add(new GoapAction.Builder("GoToTheTerrace")
            .WithStrategy(new MoveStrategy(_navMesh, KTargetDistance, () => terrace.Position))
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
            .WithStrategy(new MoveStrategy(_navMesh, KTargetDistance, () => desk.Position))
            //.AddPrecondition(_beliefs["HasDesk"])
            .AddPostCondition(_beliefs["AtDesk"])
            .Build());
        // _actions.Add(new GoapAction.Builder("WaitForDesk")
        //     .WithStrategy(new QueueStrategy(5, desk))
        //     //.AddPrecondition(_beliefs["HasDesk"])
        //     .AddPostCondition(_beliefs["HasDesk"])
        //     .Build());
        _actions.Add(new GoapAction.Builder("Work")
            .WithStrategy(new UsingMachineStrategy(3, desk, gameObject))
            .AddPrecondition(_beliefs["AtDesk"])
            .AddPostCondition(_beliefs["MakeMoney"])
            .Build());

        _actions.Add(new GoapAction.Builder("SmallTalkToBoss")
            .WithStrategy(new MoveStrategy(_navMesh, KTargetDistance, () => talkPerson.position))
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
            .WithType(BasePlace.PlaceType.None)
            .WithDesiredEffect(_beliefs["Nothing"])
            .Build());

        _goals.Add(new GoapGoal.Builder("HaveABreak")
            .WithPriority(0.1f)
            .WithType(BasePlace.PlaceType.Break)
            .WithDesiredEffect(_beliefs["HadABreak"])
            .Build());

        _goals.Add(new GoapGoal.Builder("MakeMoney")
            .WithPriority(1)
            .WithType(BasePlace.PlaceType.Work)
            .WithDesiredEffect(_beliefs["MakeMoney"])
            .Build());
        
        _goals.Add(new GoapGoal.Builder("Talk")
            .WithPriority(0.1f)
            .WithType(BasePlace.PlaceType.Social)
            .WithDesiredEffect(_beliefs["HadATalk"])
            .Build());

    }

}
