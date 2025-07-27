using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;


namespace Places
{
    namespace Queue
    {

        public class QueueCandidate : MonoBehaviour
        {
            public QueuePoint QueuePoint;
            [SerializeField] private QueueManager _queueManager;

            private NavMeshAgent _agent;
            private Vector3 _startPosition;

            private void Awake()
            {
                _agent = GetComponent<NavMeshAgent>();
                _startPosition = transform.position;
            }

            private void OnEnable()
            {
                if (_queueManager.Register(this, out QueuePoint qp))
                {
                    QueuePoint = qp;
                }
                else
                {
                    Debug.Log("Queue full");
                    QueuePoint = null;
                }
                
            }
            
            private void Update()
            {
                if (QueuePoint && _agent.destination != QueuePoint.transform.position)
                {
                    _agent.destination = QueuePoint.transform.position;
                    Debug.Log("Changing destination " + _agent.destination);
                }
                else
                {
                    _agent.destination = _startPosition;
                }
            }
            
            private void OnDisable()
            {
                if (_queueManager.Unregister(this))
                {
                    _agent.destination = _startPosition;
                }
            }

        }

    }
}
