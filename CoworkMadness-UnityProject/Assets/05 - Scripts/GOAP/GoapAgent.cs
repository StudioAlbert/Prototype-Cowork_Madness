using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace GOAP
{
    public class GoapAgent : MonoBehaviour
    {

        // Physics and regular components
        private NavMeshAgent _navMesh;
        private Rigidbody _rb;
        private Animator _animator;

        // TODO Place provider
        [Header("Places")]
        [SerializeField] private Transform desk;
        [SerializeField] private Transform coffeeMachine;
        [SerializeField] private Transform discussion;

        // TODO Mood manager => updates the priority according to goal types
        [SerializeField] private float workOverTime;
        [SerializeField] private float breakOverTime;
        [SerializeField] private float socialOverTime;

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

        private void Awake()
        {
            _navMesh = GetComponent<NavMeshAgent>();
            _rb = GetComponent<Rigidbody>();
            _animator = GetComponent<Animator>();

            _planner = new GoapPlanner();

        }

        private void Start()
        {
            SetupBeliefs();
            SetupGoals();
        }

        private void Update()
        {
            UpdatePriorities(Time.deltaTime);

            if (_currentAction == null)
            {
                Debug.Log($"{name} looking for a Plan.");
                GetAPlan();

                if (_actionPlan != null && _actionPlan.Actions.Count > 0)
                {
                    _navMesh.ResetPath();
                    _currentGoal = _actionPlan.Goal;
                    _currentAction = _actionPlan.Actions.Pop();
                    _currentAction.Start();
                    Debug.Log($"GOAL: {_currentGoal.Name} with {_actionPlan.Actions.Count} actions plammed.");
                    Debug.Log($"Popped action: {_currentAction.Name}");
                }
            }
            
            
            // Exexcute current action
            if (_actionPlan != null && _currentAction != null)
            {
                _currentAction.Update(Time.deltaTime);
                    
                    if(_currentAction.Complete)
                    {
                        Debug.Log("Currennt Action Complete !");
                        _currentAction.Stop();
                        _currentAction = null;
                        
                        // end of the plan
                        if (_actionPlan.Actions.Count == 0)
                        {
                            Debug.Log("Plan complete");
                            _lastGoal = _currentGoal;
                            _currentGoal = null;
                        }
                    }
            }
            
            
        }

        private void SetupBeliefs()
        {
            _beliefs = new Dictionary<string, GoapBelief>();
            GoapBeliefFactory bFactory = new GoapBeliefFactory(this, _beliefs);

            bFactory.AddBelief("Nothing", () => false);
            bFactory.AddBelief("AgentIdle", () => !_navMesh.hasPath);
            bFactory.AddBelief("AgentMoving", () => _navMesh.hasPath);
        }
        void SetupGoals()
        {
            _goals = new HashSet<GoapGoal>();

            _goals.Add(new GoapGoal.Builder("GetBored")
                .WithPriority(1)
                .WithType(PlaceType.Social)
                .WithDesiredEffect(_beliefs["Nothing"])
                .Build());
        }
        void GetAPlan()
        {
            var priorityLvl = _currentGoal?.Priority ?? 0;

            HashSet<GoapGoal> goalsToCheck = _goals;
            if (_currentGoal != null)
            {
                Debug.Log($"Dowe have to change current Goal : {_currentGoal.Name}|{_currentGoal.Priority} ?");
                goalsToCheck = new HashSet<GoapGoal>(_goals.Where(g => g.Priority > priorityLvl));
            }

            var potentialPlan = _planner.Plan(this, goalsToCheck, _lastGoal);
            if (potentialPlan != null)
            {
                _actionPlan = potentialPlan;
            }

        }

        void UpdatePriorities(float deltaTime)
        {

            // TODO Moodmanager
            foreach (GoapGoal goal in _goals)
            {
                switch (goal.PlaceType)
                {
                    case PlaceType.Work:
                        goal.Priority += workOverTime * Time.deltaTime;
                        break;
                    case PlaceType.Break:
                        goal.Priority += breakOverTime * Time.deltaTime;
                        break;
                    case PlaceType.Social:
                        goal.Priority += socialOverTime * Time.deltaTime;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                goal.Priority = Mathf.Clamp(goal.Priority, 0, 100);

            }

        }

    }

}
