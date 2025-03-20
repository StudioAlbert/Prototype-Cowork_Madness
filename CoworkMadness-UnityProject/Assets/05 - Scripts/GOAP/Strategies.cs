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
}
