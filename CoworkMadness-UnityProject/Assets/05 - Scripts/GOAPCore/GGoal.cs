using UnityEngine;
using UnityEngine.Serialization;

namespace GOAPCore
{
    [System.Serializable]
    public class GGoal
    {

        [SerializeField] private string _goalName;
        public string GoalName => _goalName;
        
        [SerializeField] private int _basePriority;
        private GState _goalState;
        public GState GoalState => _goalState;

        public int RelativePriority()
        {
            return _basePriority;
        }
        
        // Ctor ------------------------------------------
        public GGoal(string stateHash, int stateValue, int priority)
        {
            _goalName = stateHash;
            _basePriority = priority;
            _goalState = new GState(stateHash, stateValue);
        }
   
    }
}
