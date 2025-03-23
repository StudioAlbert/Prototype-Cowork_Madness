namespace GOAP
{
    public interface IActionStrategy
    { 
        bool CanPerform { get; }
        bool Complete { get; }

        void Start();
        void Stop();
        void Update(float deltaTime);
    }

    public class IdleStrategy : IActionStrategy
    {
        public bool CanPerform => true;
        public bool Complete { get; private set; }

        private readonly CountdownTimer _timer;

        public void Start() => _timer.Start();
        public void Stop() => _timer.Stop();
        public void Update(float deltaTime) => _timer.Tick(deltaTime);

        public IdleStrategy(float duration)
        {
            _timer = new CountdownTimer(duration);
            _timer.OnTimerStart += () => Complete = false;
            _timer.OnTimerStop += () => Complete = true;
        }
    }
}

