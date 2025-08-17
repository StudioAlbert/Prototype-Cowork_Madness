using System.Collections.Generic;
using UnityEngine;

namespace Places.Queue
{
    public abstract class BaseQueueManager : MonoBehaviour
    {
        
        protected abstract List<QueueCandidate> Candidates {get;}

        public abstract bool Register(QueueCandidate candidate);
        public abstract bool Unregister(QueueCandidate candidate);
        public abstract bool IsQueueDone(QueueCandidate candidate);
        
        public abstract bool HasFreePositions();
        
    }
}
