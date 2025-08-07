using UnityEngine;

namespace Places.Process
{
    public class TimeBasedStrategy : IProcessStrategy
    {
        private float _totalTime;
        private float _elapsedTime;
        private Status _status = Status.Unset;
        private readonly float _timeAvg;
        private readonly float _timeVar;

        public Status Status => _status;
        public float Progress => _elapsedTime / _totalTime;

        public TimeBasedStrategy(float timeAvg, float timeVar)
        {
            _timeAvg = timeAvg;
            _timeVar = timeVar;
        }

        public bool StartProcess()
        {
            _elapsedTime = 0f;
            _totalTime = Random.Range(Mathf.Max(0f, _timeAvg - _timeVar), _timeAvg + _timeVar);
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
