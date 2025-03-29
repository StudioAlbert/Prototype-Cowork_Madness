using GOAP;
using Places;

namespace AI
{
    public class UsingMachineStrategy : IGoapActionStrategy
    {
        private readonly BasePlace _basePlace;
        private readonly CountdownTimer _timer;
        
        public UsingMachineStrategy(float duration, BasePlace basePlace)
        {
            _timer = new CountdownTimer(duration);
            _timer.OnTimerStart += () => Complete = false;
            _timer.OnTimerStop += () => Complete = true;

            _basePlace = basePlace;
        }

        public bool CanPerform => true;
        public bool Complete { get; private set; }
        public float Progress { get; private set; }
        public void Start()
        {
            _timer.Start();
            _basePlace.InUse = true;
        }
        public void Stop()
        {
            _timer.Stop();
            _basePlace.InUse = false;
        }
        public void Update(float deltaTime)
        {
            _timer.Tick(deltaTime);
            Progress = _timer.Progress;
        }

    }
}
