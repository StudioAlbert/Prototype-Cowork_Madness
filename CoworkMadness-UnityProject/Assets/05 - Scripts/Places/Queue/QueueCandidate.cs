using UnityEngine;

namespace Places.Queue
{
    public class QueueCandidate : MonoBehaviour
    {
        public QueuePoint QueuePoint;

        [SerializeField] private float _abortTimeAvg = 5;
        [SerializeField] private float _abortTimeVar = 1;

        private float _elapsedTime;
        private float _abortTime;
        
        public void StartWait()
        {
            _elapsedTime = 0f;
            _abortTime = Random.Range(Mathf.Max(0f, _abortTimeAvg - _abortTimeVar), _abortTimeAvg + _abortTimeVar);
        }

        public bool CanWait(float deltaTime)
        {
            _elapsedTime += deltaTime;
            return _elapsedTime < _abortTime;
        }
    }
}
