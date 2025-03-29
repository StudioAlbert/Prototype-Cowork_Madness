using GOAP;
using Places;
using UnityEngine;

namespace AI
{
    public class UsingMachineStrategy : IGoapActionStrategy
    {
        private readonly SimplePlace _place;
        private readonly GameObject _user;
        private readonly CountdownTimer _timer;
        
        public UsingMachineStrategy(float duration, SimplePlace place, GameObject agent)
        {
            _timer = new CountdownTimer(duration);
            _timer.OnTimerStart += () => Complete = false;
            _timer.OnTimerStop += () => Complete = true;

            _place = place;
            _user = agent;
        }

        public bool CanPerform => _place.user == _user || _place.user  == null;
        public bool Complete { get; private set; }
        public float Progress => _timer.Progress;
        public void Start()
        {
            _timer.Start();
            if (_place.user == null)
            {
                _place.user = _user;
            }
        }
        public void Stop()
        {
            _timer.Stop();
            _place.user = null;
        }

        public void Update(float deltaTime)
        {
            _timer.Tick(deltaTime);
            if (_place.user == null)
            {
                _place.user = _user;
            }
        }

    }
}
