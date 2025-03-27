using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace GOAP
{
    public class GoapAgent : MonoBehaviour
    {

        // Physics and regular components
        private NavMeshAgent _navMesh;
        private Rigidbody _rb;
        private Animator _animator;
        private Inventory _inventory;

        // TODO Place provider
        [Header("Places")]
        [SerializeField] private Transform desk;
        [SerializeField] private Transform coffeeMachine;
        [SerializeField] private Transform terrace;
        [SerializeField] private Transform talkPerson;

        // GOAP Machinery
        private GoapGoal _lastGoal;
        private GoapGoal _currentGoal;
        private GoapPlan _actionPlan;
        private GoapAction _currentAction;

        private Dictionary<string, GoapBelief> _beliefs;
        private HashSet<GoapAction> _actions;
        private HashSet<GoapGoal> _goals;

        private IGoapPlanner _planner;

        public GoapGoal CurrentGoal => _currentGoal;
        public GoapPlan ActionPlan => _actionPlan;
        public GoapAction CurrentAction => _currentAction;
        public Dictionary<string, GoapBelief> Beliefs => _beliefs;
        public HashSet<GoapAction> Actions => _actions;
        public HashSet<GoapGoal> Goals => _goals;

        public event Action<GoapGoal> OnGoalDone;

        private void Awake()
        {
            _navMesh = GetComponent<NavMeshAgent>();
            _rb = GetComponent<Rigidbody>();
            _animator = GetComponent<Animator>();
            _inventory = GetComponent<Inventory>();

            _planner = new GoapPlanner();

        }

        private void Start()
        {
            SetupBeliefs();
            SetupActions();
            SetupGoals();
        }

        private void Update()
        {
            if (_currentAction == null)
            {
                Debug.Log($"{name} looking for a Plan.");
                GetAPlan();

                if (_actionPlan != null && _actionPlan.Actions.Count > 0)
                {

                    _navMesh.ResetPath();
                    _currentGoal = _actionPlan.Goal;
                    Debug.Log($"GOAL: {_currentGoal.Name} with {_actionPlan.Actions.Count} actions planned.");

                    _currentAction = _actionPlan.Actions.Pop();
                    _currentAction.Start();
                    Debug.Log($"Popped action: {_currentAction.Name}");
                }
            }


            // Execute current action
            if (_actionPlan != null && _currentAction != null)
            {
                _currentAction.Update(Time.deltaTime);

                if (_currentAction.Complete)
                {
                    Debug.Log($"Current Action {_currentAction.Name} Complete !");
                    _currentAction.Stop();
                    _currentAction = null;

                    // end of the plan
                    if (_actionPlan.Actions.Count == 0)
                    {
                        Debug.Log($"Plan complete for {_currentGoal.Name}");
                        _lastGoal = _currentGoal;
                        _currentGoal = null;

                        OnGoalDone?.Invoke(_lastGoal);

                    }
                }
            }

            void GetAPlan()
            {
                var priorityLvl = _currentGoal?.Priority ?? 0;

                HashSet<GoapGoal> goalsToCheck = _goals;
                if (_currentGoal != null)
                {
                    Debug.Log($"Do we have to change current Goal : {_currentGoal.Name}|{_currentGoal.Priority} ?");
                    goalsToCheck = new HashSet<GoapGoal>(_goals.Where(g => g.Priority > priorityLvl));
                }

                var potentialPlan = _planner.Plan(this, goalsToCheck, _lastGoal);
                if (potentialPlan != null)
                    _actionPlan = potentialPlan;

            }


        }

        private void SetupBeliefs()
        {
            _beliefs = new Dictionary<string, GoapBelief>();
            GoapBeliefFactory bFactory = new GoapBeliefFactory(this, _beliefs);

            bFactory.AddBelief("Nothing", () => false);
            bFactory.AddBelief("AgentIdle", () => !_navMesh.hasPath);
            bFactory.AddBelief("AgentMoving", () => _navMesh.hasPath);

            bFactory.AddBelief("HadABreak", () => false);
            // TODO
            // Make a better (inventory/item class) prop/NPC system with feedback => have a coffee mug in the hand
            bFactory.AddBelief("HasACoffee", () => _inventory.CoffeeEquipped);
            bFactory.AddLocationBelief("AtCoffeeMachine", 0.5f, coffeeMachine);
            bFactory.AddLocationBelief("AtTheTerrace", 0.5f, terrace);

            bFactory.AddBelief("MakeMoney", () => false);
            bFactory.AddLocationBelief("AtDesk", 0.5f, desk);
            bFactory.AddBelief("HasADesk", () => desk.gameObject.activeSelf);

            bFactory.AddBelief("HadATalk", () => false);
            bFactory.AddLocationBelief("MetSomeone", 0.5f, talkPerson);


        }

        private void SetupActions()
        {
            _actions = new HashSet<GoapAction>();

            _actions.Add(new GoapAction.Builder("Nothing")
                .WithStrategy(new IdleStrategy(0))
                .AddPostCondition(_beliefs["Nothing"])
                .Build());

            _actions.Add(new GoapAction.Builder("GetACoffee")
                .WithStrategy(new MoveStrategy(_navMesh, () => coffeeMachine.position))
                .AddPostCondition(_beliefs["AtCoffeeMachine"])
                .Build());
            _actions.Add(new GoapAction.Builder("MakeACoffee")
                .WithStrategy(new IdleStrategy(5))
                .AddPrecondition(_beliefs["AtCoffeeMachine"])
                .AddPostCondition(_beliefs["HasACoffee"])
                .AddConsequence(() => _inventory.CoffeeEquipped = true)
                .Build());
            _actions.Add(new GoapAction.Builder("GoToTheTerrace")
                .WithStrategy(new MoveStrategy(_navMesh, () => terrace.position))
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
                .WithStrategy(new MoveStrategy(_navMesh, () => desk.position))
                .AddPrecondition(_beliefs["HasADesk"])
                .AddPostCondition(_beliefs["AtDesk"])
                .Build());
            _actions.Add(new GoapAction.Builder("Work")
                .WithStrategy(new IdleStrategy(15))
                .AddPrecondition(_beliefs["AtDesk"])
                .AddPostCondition(_beliefs["MakeMoney"])
                .Build());

            _actions.Add(new GoapAction.Builder("SmallTalkToBoss")
                .WithStrategy(new MoveStrategy(_navMesh, () => talkPerson.position))
                .AddPostCondition(_beliefs["MetSomeone"])
                .Build());
            _actions.Add(new GoapAction.Builder("SmallTalkToBoss")
                .WithStrategy(new IdleStrategy(3))
                .AddPrecondition(_beliefs["MetSomeone"])
                .AddPostCondition(_beliefs["HadATalk"])
                .Build());

        }
        void SetupGoals()
        {
            _goals = new HashSet<GoapGoal>();

            _goals.Add(new GoapGoal.Builder("Nothing")
                .WithPriority(0.01f)
                .WithType(PlaceType.None)
                .WithDesiredEffect(_beliefs["Nothing"])
                .Build());

            _goals.Add(new GoapGoal.Builder("HaveABreak")
                .WithPriority(0.1f)
                .WithType(PlaceType.Break)
                .WithDesiredEffect(_beliefs["HadABreak"])
                .Build());

            _goals.Add(new GoapGoal.Builder("MakeMoney")
                .WithPriority(1)
                .WithType(PlaceType.Work)
                .WithDesiredEffect(_beliefs["MakeMoney"])
                .Build());
            
            _goals.Add(new GoapGoal.Builder("Talk")
                .WithPriority(0.1f)
                .WithType(PlaceType.Social)
                .WithDesiredEffect(_beliefs["HadATalk"])
                .Build());

        }

    }

}
