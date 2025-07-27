using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace Places
{
    namespace Queue
    {

        public class QueueManager : MonoBehaviour
        {

            [SerializeField] private List<QueuePoint> _queuePoints = new List<QueuePoint>();

            private List<QueueCandidate> _candidates = new List<QueueCandidate>();
            
            public bool Register(QueueCandidate candidate, out QueuePoint queuePoint)
            {
                foreach (var qp in _queuePoints)
                {
                    if (!qp.Occupied)
                    {
                        qp.Occupied = true;
                        queuePoint = qp;
                        _candidates.Add(candidate);
                        return true;
                    }
                }
                
                queuePoint = null;
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
                _candidates[idxFound].QueuePoint.Occupied = false;
                _candidates[idxFound].QueuePoint = null;
                _candidates.RemoveAt(idxFound);
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
            
        }
        
    }
}
