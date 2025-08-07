using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Places.Queue
{

    public class QueueManager : MonoBehaviour
    {

        [SerializeField] private List<QueuePoint> _queuePoints = new List<QueuePoint>();
        [SerializeField] private QueuePoint _doneQueuePoint;
        
        private readonly List<QueueCandidate> _candidates = new List<QueueCandidate>();

        public bool Register(QueueCandidate candidate)
        {
            foreach (var qp in _queuePoints)
            {
                if (!qp.Occupied)
                {
                    qp.Occupied = true;
                    candidate.QueuePoint = qp;
                    _candidates.Add(candidate);
                    return true;
                }
            }

            candidate.QueuePoint = null;
            return false;
        }

        public bool Unregister(QueueCandidate candidate)
        {
            int idxFound = -1;

            for (int i = 0; i < _candidates.Count; i++)
            {
                if (_candidates[i].Equals(candidate))
                {
                    idxFound = i;
                    break;
                }
            }

            if (idxFound == -1) return false;

            // Clean datas of lost candidate
            candidate.QueuePoint.Occupied = false;
            candidate.QueuePoint = null;
            _candidates.Remove(candidate);
            // Reallocate queue points
            for (int j = idxFound; j < _candidates.Count; j++)
            {
                _candidates[j].QueuePoint = _queuePoints[j];
                _candidates[j].QueuePoint.Occupied = true;
            }
            // Free from occupied remaining queue points
            for (int j = _candidates.Count; j < _queuePoints.Count; j++)
            {
                _queuePoints[j].Occupied = false;
            }

            return true;

        }

        public bool HasFreePositions()
        {
            return _candidates.Count < _queuePoints.Count;
        }
        public Vector3 EntryPoint()
        {
            return _queuePoints.Last().transform.position;
        }
        public bool IsQueueDone(QueueCandidate candidate)
        {
            return candidate.QueuePoint == _doneQueuePoint;
        }
    }


}
