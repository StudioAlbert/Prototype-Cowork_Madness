using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Places.Queue
{
    public class ManyToOneQueueManager : BaseQueueManager
    {

        [SerializeField] private List<QueuePoint> _queuePoints = new List<QueuePoint>();
        [SerializeField] private QueuePoint _actionPoint;

        private readonly List<QueueCandidate> _candidates = new List<QueueCandidate>();
        protected override List<QueueCandidate> Candidates => _candidates;

        // private void Start()
        // {
        //     _allQueuePoints = new List<QueuePoint>(_doneQueuePoints);
        //     _allQueuePoints.AddRange(_queuePoints);
        // }
        private bool PickAnActionPoint(out QueuePoint actionPoint)
        {
            if (!_actionPoint.Occupied)
            {
                actionPoint = _actionPoint;
            }
            else
            {
                actionPoint = null;
            }
            return actionPoint != null;
        }

        private bool PickAQueuePoint(out QueuePoint queuePoint)
        {
            queuePoint = _queuePoints.FirstOrDefault(p => !p.Occupied);
            return queuePoint != null;
        }

        public override bool Register(QueueCandidate candidate)
        {
            PickAnActionPoint(out candidate.QueuePoint);
            if (!candidate.QueuePoint) PickAQueuePoint(out candidate.QueuePoint);

            if (candidate.QueuePoint)
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

            // Reallocate queue points
            foreach (QueuePoint qp in _queuePoints)
            {
                if (!qp.Occupied) continue;

                QueueCandidate c = _candidates.FirstOrDefault(c => c.QueuePoint == qp);
                if (c)
                {
                    if (PickAnActionPoint(out c.QueuePoint))
                    {
                        c.QueuePoint.Occupied = true;
                        qp.Occupied = false;
                    }
                    else if (PickAQueuePoint(out c.QueuePoint))
                    {
                        c.QueuePoint.Occupied = true;
                        qp.Occupied = false;
                    }
                    else
                    {
                        qp.Occupied = false;
                        break;
                    }
                }

            }

            return true;
        }
        public override bool IsQueueDone(QueueCandidate candidate)
        {
            return _actionPoint == candidate.QueuePoint;
        }
        public override bool HasFreePositions()
        {
            return _queuePoints.Exists(qp => !qp.Occupied) || !_actionPoint.Occupied;
        }
        
    }
}
