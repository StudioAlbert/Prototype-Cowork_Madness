using System;
using AI_Motivation;
using Places.Process;
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
        
        [Header("Processing")]
        [SerializeField] private float _processingTimeAvg = 7.5f;
        [SerializeField] private float _processingTimeVar = 4.5f;
        [SerializeField] private bool _canAbort = false;

        [Header("Attributes")]
        [SerializeField] private Attributes.Quality _quality = Attributes.Quality.Bronze;
        
        private QueuePoint _point;
        private IProcessStrategy _processStrategy;

        protected override PlaceProvider PlaceProvider
        {
            get => _placeProvider;
            set => _placeProvider = value;
        }
        public override bool Available => _queueManager.HasFreePositions();
        public override GoalType Type => _type;
        public override Vector3 Position => _queueManager.EntryPoint();
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
        
        protected override IProcessStrategy ProcessStrategy
        {
            get => _processStrategy;
            set => _processStrategy = value;
        }
        public override Status ProcessStatus => _processStrategy.Status;
        public override float ProcessProgress => _processStrategy.Progress;
        public override void StartProcess() => _processStrategy.StartProcess();
        public override void Process(float deltaTime) => _processStrategy.Process(deltaTime);
        public override void StopProcess() => _processStrategy.StopProcess();
        public override bool CanAbort => _canAbort;
        
        public override Attributes.Quality Quality => _quality;
        
        private void Start()
        {
            if(!_queueManager) _queueManager = GetComponent<QueueManager>();
            ProcessStrategy = new TimeBasedStrategy(_processingTimeAvg, _processingTimeVar);
        }

        
        public void OnDrawGizmos()
        {
            Gizmos.color = Available ? Color.green : Color.red;
            Gizmos.DrawWireSphere(Position, Neighbourhood);
        }
    }
}
