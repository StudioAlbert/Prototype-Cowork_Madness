using GOAP;
using Places;
using UnityEngine;

namespace AI
{
    public class UsingMachineStrategy : IGoapActionStrategy
    {
        private readonly SimplePlace _place;
        private readonly GameObject _agent;
        private readonly CountdownTimer _timer;
        
        public UsingMachineStrategy(float duration, SimplePlace place, GameObject agent)
        {
            _timer = new CountdownTimer(duration);
            _timer.OnTimerStart += () => Complete = false;
            _timer.OnTimerStop += () => Complete = true;

            _place = place;
            _agent = agent;
        }

        //public bool CanPerform => true;
        public bool CanPerform => _place.User == _agent || !_place.Available;
        public bool Complete { get; private set; }
        public bool Failed => false;
        public float Progress => _timer.Progress;
        public void Start()
        {
            _timer.Start();
            if (!_place.Available)
            {
                _place.RegisterUser(_agent);
            }
        }
        public void Stop()
        {
            _timer.Stop();
            _place.UnregisterUser(_agent);;
        }

        public void Update(float deltaTime)
        {
            _timer.Tick(deltaTime);
            if (!_place.Available)
            {
                _place.RegisterUser(_agent);;
            }
        }

    }
}
