using System;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.AI;

namespace GOAP
{
    public interface IGoapActionStrategy
    { 
        bool CanPerform { get; }
        bool Complete { get; }
        float Progress { get; }

        void Start();
        void Stop();
        void Update(float deltaTime);
    }

    public class IdleStrategy : IGoapActionStrategy
    {
        private readonly CountdownTimer _timer;
        
        public IdleStrategy(float duration)
        {
            _timer = new CountdownTimer(duration);
            _timer.OnTimerStart += () => Complete = false;
            _timer.OnTimerStop += () => Complete = true;
        }
        
        public bool CanPerform => true;
        public bool Complete { get; private set; }
        public float Progress { get; private set; }

        public void Start() => _timer.Start();
        public void Stop() => _timer.Stop();
        public void Update(float deltaTime)
        {
            _timer.Tick(deltaTime);
            Progress = _timer.Progress;
        }

    }
    
    public class MoveStrategy : IGoapActionStrategy {
        private readonly NavMeshAgent _agent;
        private readonly Func<Vector3> _destination;
    
        public bool CanPerform => !Complete;
        public bool Complete => _agent.remainingDistance <= 2f && !_agent.pathPending;
        public float Progress { get; private set; }

        private float _startDistance;
        
        public MoveStrategy(NavMeshAgent agent, Func<Vector3> destination) {
            this._agent = agent;
            this._destination = destination;
        }      
    
        public void Start()
        {
            _startDistance = Vector3.Distance(_agent.transform.position, _destination());
            _agent.SetDestination(_destination());
        }
        public void Stop() => _agent.ResetPath();
        public void Update(float deltaTime)
        { 
            Progress = _agent.remainingDistance / _startDistance;
        }
    }
}

