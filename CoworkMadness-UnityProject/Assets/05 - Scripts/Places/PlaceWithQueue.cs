using System;
using AI_Motivation;
using Places.Queue;
using UnityEngine;

namespace Places
{
    public class PlaceWithQueue : BasePlace
    {
        [Header("Places")]
        [SerializeField] private PlaceProvider _placeProvider;
        [SerializeField] private GoalType _type;
        
        [Header("Queue")]
        [SerializeField] private QueueManager _queueManager;
        [SerializeField] private float _neighbourhood = 5f;
        private QueuePoint _point;

        protected override PlaceProvider PlaceProvider
        {
            get => _placeProvider;
            set => _placeProvider = value;
        }
        public override bool Available => _queueManager.HasFreePositions();
        public override GoalType Type => _type;
        public override Vector3 Position => _queueManager.FirstFreePosition();
        public override float Neighbourhood => _neighbourhood;
        public override bool RegisterUser(GameObject user)
        {
            if (user.TryGetComponent(out QueueCandidate candidate))
            {
                return _queueManager.Register(candidate);
            }
            return false;
            // TODO : What to do with queue point ?
        }
        public override bool UnregisterUser(GameObject user)
        {
            if (user.TryGetComponent(out QueueCandidate candidate))
            {
                return _queueManager.Unregister(candidate);
            }
            return false;
        }

        private void Start()
        {
            if(!_queueManager) _queueManager = GetComponent<QueueManager>();
        }
        
        public void OnDrawGizmos()
        {
            Gizmos.color = Available ? Color.green : Color.red;
            Gizmos.DrawWireSphere(Position, Neighbourhood);
        }
    }
}
