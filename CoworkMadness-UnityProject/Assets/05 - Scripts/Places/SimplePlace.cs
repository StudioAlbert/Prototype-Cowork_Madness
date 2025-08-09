using AI_Motivation;
using Places.Process;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace Places
{
        public class SimplePlace : BasePlace
        {
            [SerializeField] private GoalType _type;
            [SerializeField] private GameObject _user;
            [SerializeField] private PlaceProvider _placeProvider;
            [SerializeField] private float _neighbourhood = 5f;
            [Header("Processing")]
            [SerializeField] private float _processingTimeAvg = 7.5f;
            [SerializeField] private float _processingTimeVar = 4.5f;
            [SerializeField] private bool _canAbort;

            public GameObject User => _user;
            private IProcessStrategy _processStrategy;

            protected override PlaceProvider PlaceProvider
            {
                get => _placeProvider; 
                set => _placeProvider = value;
            }
            public override bool Available =>  _user == null;
            public override GoalType Type => _type;
            public override Vector3 Position => transform.position;
            public override float Neighbourhood => _neighbourhood;

            public override bool RegisterUser(GameObject user)
            {
                _user = user;
                return true;
            }
            public override bool UnregisterUser(GameObject user)
            {
                _user = null;
                return true;
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
            
            private void Start()
            {
                ProcessStrategy = new TimeBasedStrategy(_processingTimeAvg, _processingTimeVar);
            }

        }
}
