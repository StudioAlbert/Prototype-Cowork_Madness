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
        bool Failed { get; }
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
        public bool Failed => false;
        public float Progress => _timer.Progress;

        public void Start() => _timer.Start();
        public void Stop() => _timer.Stop();
        public void Update(float deltaTime)
        {
            _timer.Tick(deltaTime);
        }

    }
    
    public class MoveStrategy : IGoapActionStrategy {
        private readonly NavMeshAgent _navMesh;
        private readonly float _distance;
        private readonly Func<Vector3> _destination;
    
        public bool CanPerform => !Complete;
        public bool Complete => _navMesh.remainingDistance <= _distance && !_navMesh.pathPending;
        public bool Failed => false;
        public float Progress =>  _navMesh.remainingDistance / _startDistance;

        private float _startDistance;

        public MoveStrategy(NavMeshAgent navMesh, float distance, Func<Vector3> destination) {
            _navMesh = navMesh;
            _distance = distance;
            _destination = destination;
        }      
    
        public void Start()
        {
            _startDistance = Vector3.Distance(_navMesh.transform.position, _destination());
            _navMesh.SetDestination(_destination());
        }
        public void Stop() => _navMesh.ResetPath();
        public void Update(float deltaTime)
        { 
            // No update
        }
    }
}

