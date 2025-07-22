using System;
using GOAP;
using Places;
using UnityEngine;

namespace AI
{
    //[Obsolete("TODO : Queue attitude", true)]
    public class QueueStrategy : IGoapActionStrategy
    {
        private readonly CountdownTimer _timer;
        private readonly SimplePlace _place;
        private readonly GameObject _agent;
        
        public QueueStrategy(float duration, SimplePlace place, GameObject agent)
        {
            _place = place;
            _agent = agent;
            
            _timer = new CountdownTimer(duration);
            _timer.OnTimerStart += () =>
            {
                Complete = false;
                Failed = false;
            };
            _timer.OnTimerStop += () =>
            {
                Complete = (_place.User == _agent);
                Failed = !(_place.User == _agent);
            };
        }
        
        public bool CanPerform => true;
        public bool Complete { get; private set; }
        public bool Failed { get; private set; }
        public float Progress => _timer.Progress;

        public void Start() => _timer.Start();
        public void Stop()
        {
            _timer.Stop();
        }
        public void Update(float deltaTime)
        {
            _timer.Tick(deltaTime);

            // ReSharper disable once InvertIf
            if (_place.Available)
            {
                _place.RegisterUser(_agent);
                Stop();
            }
        }

    }
}
