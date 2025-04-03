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
        
        public QueueStrategy(float duration)
        {
            _timer = new CountdownTimer(duration);
            _timer.OnTimerStart += () => Complete = false;
            _timer.OnTimerStop += () => Complete = true;
        }
        
        public bool CanPerform => true;
        public bool Complete { get; private set; }
        public bool Failed => false;
        public float Progress => _timer.Progress;

        public void Start() => _timer.Start();
        public void Stop() => _timer.Stop();
        public void Update(float deltaTime)
        {
            _timer.Tick(deltaTime);
        }

    }
}
