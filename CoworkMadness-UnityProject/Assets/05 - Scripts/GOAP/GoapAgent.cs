using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Utilities;

namespace GOAP
{
    [RequireComponent(typeof(NavMeshAgent))]
    public abstract class GoapAgent : MonoBehaviour
    {
        // Loggers
        private LoggerObject _loggerObject;
        
        // Physics and regular components
        protected NavMeshAgent _navMesh;
        // GOAP Machinery
        private GoapGoal _lastGoal;
        private GoapGoal _currentGoal;
        private GoapPlan _actionPlan;
        private GoapAction _currentAction;

        protected Dictionary<string, GoapBelief> _beliefs;
        protected HashSet<GoapAction> _actions;
        protected HashSet<GoapGoal> _goals;

        protected IGoapPlanner _planner;

        public GoapGoal CurrentGoal => _currentGoal;
        public GoapPlan ActionPlan => _actionPlan;
        public GoapAction CurrentAction => _currentAction;
        public Dictionary<string, GoapBelief> Beliefs => _beliefs;
        public HashSet<GoapAction> Actions => _actions;
        public HashSet<GoapGoal> Goals => _goals;

        public event Action<GoapGoal> OnGoalDone;

        private void Start()
        {

            _loggerObject = GetComponent<LoggerObject>();
            
            SetupBeliefs();
            SetupActions();
            SetupGoals();
        }

        private void Update()
        {
            // Update the plan and current action if there is one
            if (_currentAction == null)
            {
                _loggerObject.Log("Calculating any potential new plan");
                GetAPlan();

                if (_actionPlan != null && _actionPlan.Actions.Count > 0)
                {
                    _navMesh.ResetPath();

                    _currentGoal = _actionPlan.Goal;
                    _loggerObject.Log($"Goal: {_currentGoal.Name} with {_actionPlan.Actions.Count} actions in plan");
                    _currentAction = _actionPlan.Actions.Pop();
                    _loggerObject.Log($"Popped action: {_currentAction.Name}");
                    
                    // Verify all precondition effects are true
                    if (_currentAction.Preconditions.All(b =>
                        {
                            _loggerObject.Log($"Belief {b.Name} : " + b.Evaluate());
                            return b.Evaluate();
                        })
                    )
                    {
                        _currentAction.Start();
                    }
                    else
                    {
                        _loggerObject.Log("Preconditions not met, clearing current action and goal");
                        _currentAction = null;
                        ResetGoal();
                    }
                }
            }


            // Execute current action
            if (_actionPlan != null && _currentAction != null)
            {
                _currentAction.Update(Time.deltaTime);

                if (_currentAction.Complete)
                {
                    _loggerObject.Log($"{_currentAction.Name} complete");
                    _currentAction.Stop();
                    _currentAction = null;

                    if (_actionPlan.Actions.Count == 0)
                    {
                        _loggerObject.Log("Plan complete");
                        ResetGoal();
                    }
                }
            }
        }

        void GetAPlan()
        {
            var priorityLvl = _currentGoal?.Priority ?? 0;

            List<GoapGoal> goalsToCheck = _goals.OrderByDescending(g => g.Priority).Where(g => g.Priority > priorityLvl).ToList();
            // if (_currentGoal != null)
            // {
            //     Debug.Log($"Do we have to change current Goal : {_currentGoal.Name}|{_currentGoal.Priority} ?");
            //     goalsToCheck = new HashSet<GoapGoal>(_goals.Where(g => g.Priority > priorityLvl));
            // }

            var potentialPlan = _planner.GetAPlan(this, goalsToCheck, _lastGoal);
            if (potentialPlan != null)
            {
                _actionPlan = potentialPlan;
                _loggerObject.Log($"{gameObject.name} found new plan : {_actionPlan.Goal.Name}");
            }
            else
            {
                _loggerObject.Log($"{gameObject.name} keep the plan : {_actionPlan.Goal.Name}");
            }
        }

        private void ResetGoal()
        {

            _loggerObject.Log($"Reset Plan : " + (_currentGoal != null ? _currentGoal.Name : "No goal"));
            if (_currentGoal == null)
                return;

            _lastGoal = _currentGoal;
            _currentGoal = null;
            OnGoalDone?.Invoke(_lastGoal);

        }

        protected abstract void SetupBeliefs();
        protected abstract void SetupActions();
        protected abstract void SetupGoals();
    }

}
