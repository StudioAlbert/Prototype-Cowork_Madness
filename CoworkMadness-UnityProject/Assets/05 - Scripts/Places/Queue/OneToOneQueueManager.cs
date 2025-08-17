using System.Collections.Generic;
using System.Linq;
using UnityEditorInternal;
using UnityEngine;

namespace Places.Queue
{
    public class OnTonOneQueueManager : BaseQueueManager
    {

        [SerializeField] private QueuePoint _actionPoint;

        private QueueCandidate _candidate;
        protected override List<QueueCandidate> Candidates => null;

        private bool PickAnActionPoint(out QueuePoint actionPoint)
        {
            if (!_actionPoint.Occupied)
                actionPoint = _actionPoint;
            else
                actionPoint = null;
            
            return actionPoint != null;
        }

        public override bool Register(QueueCandidate candidate)
        {
            if (PickAnActionPoint(out candidate.QueuePoint))
            {
                candidate.QueuePoint.Occupied = true;
                _candidate = candidate;
                return true;
            }
            return false;
        }

        public override bool Unregister(QueueCandidate candidate)
        {
            if (!candidate.QueuePoint || _candidate != candidate) return false;

            // Clean datas of lost candidate
            candidate.QueuePoint.Occupied = false;
            candidate.QueuePoint = null;
            _candidate = candidate;

            return true;

        }
        public override bool IsQueueDone(QueueCandidate candidate) => true;
        public override bool HasFreePositions()
        {
            return !_actionPoint.Occupied;
        }
        
    }
}
