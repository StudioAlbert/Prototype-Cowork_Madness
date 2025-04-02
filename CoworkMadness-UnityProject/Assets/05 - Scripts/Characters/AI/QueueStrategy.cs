using System;
using GOAP;
using Places;
using UnityEngine;

namespace AI
{
    [Obsolete("TODO : Queue attitude", true)]
    public class QueueStrategy : IGoapActionStrategy
    {
        private readonly CountdownTimer _timer;
        private readonly SimplePlace _place;
        
        public QueueStrategy(float timeout, SimplePlace place)
        {
            _place = place;
            
            _timer = new CountdownTimer(timeout);
            _timer.OnTimerStart += () =>
            {
                Complete = false;
                Failed = false;
            };
            _timer.OnTimerStop += () =>
            {
                Failed = true;
                // If timer finishes, the action is Failed
                Debug.Log("Wait failed !!!!!");
            };
        }
        
        public bool CanPerform => true;
        public bool Complete { get; private set; }
        public bool Failed { get; private set; }
        public float Progress => _timer.Progress;

        public void Start() => _timer.Start();
        public void Stop() => _timer.Stop();
        public void Update(float deltaTime)
        {
            _timer.Tick(deltaTime);
            if (_place.Available)
            {
                // success
                Complete = true;
            }
        }
    }
}
