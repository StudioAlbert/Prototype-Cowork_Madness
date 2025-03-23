using System;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.AI;

namespace GOAP
{
    public interface IActionStrategy
    { 
        bool CanPerform { get; }
        bool Complete { get; }

        void Start();
        void Stop();
        void Update(float deltaTime)
        {
            // not mandatory for every implementations
        }
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
    
    public class MoveStrategy : IActionStrategy {
        readonly NavMeshAgent agent;
        readonly Func<Vector3> destination;
    
        public bool CanPerform => !Complete;
        public bool Complete => agent.remainingDistance <= 2f && !agent.pathPending;
    
        public MoveStrategy(NavMeshAgent agent, Func<Vector3> destination) {
            this.agent = agent;
            this.destination = destination;
        }      
    
        public void Start() => agent.SetDestination(destination());
        public void Stop() => agent.ResetPath();
        
    }
}

