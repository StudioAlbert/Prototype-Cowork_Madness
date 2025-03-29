using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace GOAP
{
    [RequireComponent(typeof(NavMeshAgent))]
    public abstract class GoapAgent : MonoBehaviour
    {

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

        protected abstract void SetupBeliefs();
        protected abstract void SetupActions();
        protected abstract void SetupGoals();
    }

}
