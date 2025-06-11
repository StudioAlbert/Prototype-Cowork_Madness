using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Utilities;

namespace GOAP
{
    [RequireComponent(typeof(NavMeshAgent))]
    public abstract class GoapAgent : MonoBehaviour
    {
        // Loggers
        protected LoggerObject _logger;
        
        // Physics and regular components
        protected NavMeshAgent _navMesh;
        // GOAP Machinery
        private GoapGoal _lastGoal;
        private GoapPlan _actionPlan;

        protected Dictionary<string, GoapBelief> _beliefs;

        protected IGoapPlanner _planner;
        
        [SerializeField][DisallowNull] private GoapGoal _currentGoal;
        [SerializeField] protected List<GoapGoal> _goals;
        [SerializeField][DisallowNull] private GoapAction _currentAction;
        [SerializeField] protected List<GoapAction> _actions;

        //public GoapGoal CurrentGoal => _currentGoal;
        // public GoapPlan ActionPlan => _actionPlan;
        // public Dictionary<string, GoapBelief> Beliefs => _beliefs;
        // public GoapAction CurrentAction => _currentAction;
        public List<GoapAction> Actions => _actions;
        public List<GoapGoal> Goals => _goals;

        public event Action<GoapGoal> OnGoalDone;

        private void Start()
        {
            SetupBeliefs();
            SetupActions();
            SetupGoals();
        }

        private void Update()
        {
            // Update the plan and current action if there is one
            if (_currentAction.Status is GoapStatus.ReadyToExecute or GoapStatus.Invalid)
            {
                _logger.Log("Calculating any potential new plan");
                GetAPlan();

                if (_actionPlan != null && _actionPlan.Actions.Count > 0)
                {
                    _navMesh.ResetPath();

                    _currentGoal = _actionPlan.Goal;
                    _currentGoal.SetInProgress();
                    _logger.Log($"Goal: {_currentGoal.Name} with {_actionPlan.Actions.Count} actions in plan");
                    
                    _currentAction = _actionPlan.Actions.Pop();
                    _logger.Log($"Popped action: {_currentAction.Name}");
                    
                    // Verify all precondition effects are true
                    if (_currentAction.Preconditions.All(b =>
                        {
                            _logger.Log($"Belief {b.Name} : " + b.Evaluate());
                            return b.Evaluate();
                        })
                    )
                    {
                        _currentAction.Start();
                    }
                    else
                    {
                        _logger.Log("Preconditions not met, clearing current action and goal");
                        _currentAction.Dismiss();
                        ResetGoal();
                    }
                }
            }


            // Execute current action
            if (_actionPlan != null && _currentAction.Status == GoapStatus.InProgress)
            {
                _currentAction.Update(Time.deltaTime);

                if (_currentAction.Status == GoapStatus.Complete)
                {
                    _logger.Log($"{_currentAction.Name} complete");
                    _currentAction.Stop();

                    if (_actionPlan.Actions.Count == 0)
                    {
                        _logger.Log("Plan complete");
                        ResetGoal();
                    }
                }
            }
        }

        void GetAPlan()
        {   
            //var priorityLvl = _currentGoal?.Priority ?? 0;
            // If invalid, priority = 0 => new goal
            // If valid, priority = current priority
            var priorityLvl = _currentGoal.Status != GoapStatus.Invalid ? _currentGoal.Priority : 0;

            List<GoapGoal> goalsToCheck = _goals.OrderByDescending(g => g.Priority).Where(g => g.Priority > priorityLvl).ToList();
            // if (_currentGoal != null)
            // {
            //     Debug.Log($"Do we have to change current Goal: {_currentGoal.Name}|{_currentGoal.Priority} ?");
            //     goalsToCheck = new HashSet<GoapGoal>(_goals.Where(g => g.Priority > priorityLvl));
            // }

            var potentialPlan = _planner.GetAPlan(this, goalsToCheck, _lastGoal);
            if (potentialPlan != null)
            {
                _actionPlan = potentialPlan;
                _logger.Log($"{gameObject.name} found new plan : {_actionPlan.Goal.Name}");
            }
            else
            {
                _logger.Log($"{gameObject.name} keep the plan : {_actionPlan.Goal.Name}");
            }
        }

        private void ResetGoal()
        {

            _logger.Log($"Reset Plan : " + (_currentGoal != null ? _currentGoal.Name : "No goal"));
            if (_currentGoal.Status != GoapStatus.Invalid)
            {
                _lastGoal = _currentGoal;
                _currentGoal.Dismiss();
                OnGoalDone?.Invoke(_lastGoal);
            }

        }

        protected abstract void SetupBeliefs();
        protected abstract void SetupActions();
        protected abstract void SetupGoals();
    }

}
