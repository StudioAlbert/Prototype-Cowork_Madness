using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace GOAPCore
{
    public class GAgent : MonoBehaviour
    {
        [SerializeField] private string _agentName;
        public string AgentName => _agentName;
        [SerializeField] private NavMeshAgent _navMeshAgent;
        public NavMeshAgent NavMesh => _navMeshAgent;

        private List<GState> _beliefs = new List<GState>();
        public List<GState> Beliefs => _beliefs;

        [SerializeField] private List<GAction> _actionsSet;
        [SerializeField] private List<GGoal> _goalsSet;
        private List<GGoal> _actualGoalsSet;

        private GPlanner _gPlanner;
        private Stack<GAction> _currentActions = new Stack<GAction>();
        private GGoal _currentGoal;
        private GAction _currentAction;

        // Start is called before the first frame update
        void Start()
        {

            _gPlanner = new GPlanner();

            // ---------------------------------------------------------------------
            _actionsSet = GetComponentsInChildren<GAction>().Where(a => a.enabled == true).ToList();
            foreach (GAction action in _actionsSet)
            {
                action.RegisterAgent(this);
            }

            // Pick the most important goal --------------------------------------
            _actualGoalsSet = new List<GGoal>(_goalsSet);

            RePlan();

        }

        private void RePlan()
        {
            Debug.Log(AgentName + " ==== Replan =====");

            GGoal goalCandidate = _actualGoalsSet.OrderBy(g => g.RelativePriority()).FirstOrDefault();
            _currentAction = null;

            if (goalCandidate != null)
            {
                //_actualGoalsSet.Remove(_currentGoal);

                _currentGoal = goalCandidate;
                _currentActions = _gPlanner.FindAPlan(_actionsSet, _currentGoal);
                NextAction();

                Debug.Log("Current goal : " + _currentGoal.GoalName);
                foreach (GAction a in _currentActions)
                {
                    Debug.Log("Action available : " + a.ActionName);
                }
                if (_currentAction != null)
                    Debug.Log("Current action : " + _currentAction.ActionName);
                else
                    Debug.Log("No action picked");

            }
            else
            {

                Debug.Log("No Goal available anymore");
            }
        }

        private void NextAction()
        {
            if (_currentActions.Count > 0)
            {
                if (_currentAction != null)
                    _currentAction.ActionHasEnded -= NextAction;

                GAction candidateAction = _currentActions.Pop();
                if (candidateAction != null)
                {

                    _currentAction = candidateAction;
                    _currentAction.PrePerform();
                    _currentAction.ActionHasEnded += NextAction;

                    // if (_currentAction.CanPerform())
                    // {
                    //     _currentAction.PrePerform();
                    //     _currentAction.ActionHasEnded += NextAction;
                    // }
                    // else
                    // {
                    //     RePlan();
                    // }
                }

            }
            else
            {
                RePlan();
            }
        }

        // Update is called once per frame
        void LateUpdate()
        {

            if (_currentAction != null)
            {
                Debug.Log(AgentName + " : Current action : " + _currentAction.ActionName + " [" +
                          _currentAction.CanPerform() + "]");
                
                if (_currentAction.CanPerform())
                    _currentAction.Perform();
            }

        }


    }
}
