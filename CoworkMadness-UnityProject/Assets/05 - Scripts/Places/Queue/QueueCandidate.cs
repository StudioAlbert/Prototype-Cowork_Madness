using UnityEngine;

namespace Places.Queue
{
    public abstract class QueueCandidate : MonoBehaviour
    {
        protected abstract QueueManager QueueManager { get; set; }

        public QueuePoint QueuePoint { get; set; }
        public abstract void Register();
        public abstract void Unregister();
    }
}
