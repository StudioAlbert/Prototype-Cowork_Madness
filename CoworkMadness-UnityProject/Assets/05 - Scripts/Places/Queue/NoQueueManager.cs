using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Places.Queue
{
    public class NoQueueManager : BaseQueueManager
    {

        [SerializeField] private List<QueuePoint> _actionPoints = new List<QueuePoint>();
        
        private readonly List<QueueCandidate> _candidates = new List<QueueCandidate>();
        protected override List<QueueCandidate> Candidates => _candidates;
        
        private bool PickAnActionPoint(out QueuePoint actionPoint)
        {
            actionPoint = _actionPoints.OrderBy(p => Random.value).FirstOrDefault(p => !p.Occupied);
            return actionPoint != null;
        }
        
        public override bool Register(QueueCandidate candidate)
        {
            if (PickAnActionPoint(out candidate.QueuePoint ))
            {
                candidate.QueuePoint.Occupied = true;
                _candidates.Add(candidate);
                return true;
            }
            return false;
        }
        
        public override bool Unregister(QueueCandidate candidate)
        {
            if(!candidate.QueuePoint || !_candidates.Contains(candidate)) return false;
            
            // Clean datas of lost candidate
            candidate.QueuePoint.Occupied = false;
            candidate.QueuePoint = null;
            _candidates.Remove(candidate);
            
            return true;

        }
        public override bool IsQueueDone(QueueCandidate candidate) => true;
        public override bool HasFreePositions()
        {
            return _actionPoints.Exists(qp => !qp.Occupied);
        }
        
    }
}
