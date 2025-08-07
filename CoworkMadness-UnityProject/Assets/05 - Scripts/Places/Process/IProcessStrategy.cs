namespace Places.Process
{
    public enum Status
    {
        Unset,
        Ready,
        InProgress,
        Success,
        Failed
    }
    
    public interface IProcessStrategy
    {
        public bool StartProcess();
        public void Process(float deltaTime);
        public void StopProcess();
        public Status Status { get; }
        public float Progress { get; }
    }
    
}
