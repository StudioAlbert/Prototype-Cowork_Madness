using UnityEngine;

namespace Places.Process
{
    public class TimeBasedStrategy : IProcessStrategy
    {
        private float _totalTime;
        private float _elapsedTime;
        private Status _status = Status.Unset;

        public Status Status => _status;
        public float Progress => _elapsedTime / _totalTime;

        public bool StartProcess()
        {
            _elapsedTime = 0f;
            _totalTime = Random.Range(3f, 12f);
            _status = Status.Ready;
            return true;
        }

        public void Process(float deltaTime)
        {

            _elapsedTime += deltaTime;

            if (_elapsedTime >= _totalTime)
            {
                _elapsedTime = 0f;
                _status = Status.Success;
            }
            else
            {
                _status = Status.InProgress;
            }
        }

        public void StopProcess()
        {
            _elapsedTime = 0f;
            _status = Status.Unset;
        }
    }

}
