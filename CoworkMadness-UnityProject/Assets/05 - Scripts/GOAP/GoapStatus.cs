using UnityEngine;

namespace GOAP
{
    public enum GoapStatus
    {
        Invalid,        // NOT FILLED or CAN NOT BE DONE (Preconditions)
        ReadyToExecute, // WAITING to be executed by a plan
        InProgress,     // RUNNING, was started
        Complete,       // DONE, with success
        Failed          // DONE, with failure
    }
    
    public abstract class HasGoapStatus
    {
        
        [SerializeField]
        protected GoapStatus _status = GoapStatus.Invalid;
        public GoapStatus Status => _status;
        public void Dismiss() => _status = GoapStatus.Invalid;

    }
}