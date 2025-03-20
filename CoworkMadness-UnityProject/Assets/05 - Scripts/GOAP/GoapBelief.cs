using System;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP
{
    public class GoapBelief
    {
        GoapBelief(string name)
        {
            Name = name;
        }
        
        // Is the belief True or False
        private Func<bool> _condition = () => false;
        // Some place where the belief can be true
        private Func<Vector3> _location = () => Vector3.zero;
        
        public string Name { get; }
        public Vector3 Location => _location();
        public bool Evaluate() => _condition();

        #region Builder

        public class Builder
        {
            private readonly GoapBelief _belief;

            public Builder(string name)
            {
                _belief = new GoapBelief(name);
            }
            public Builder WithCondition(Func<bool> condition)
            {
                _belief._condition = condition;
                return this;
            }
            public Builder WithLocation(Func<Vector3> location)
            {
                _belief._location = location;
                return this;
            }
            public GoapBelief Build()
            {
                return _belief;
            }
        }

        #endregion
        
    }
    
    public class GoapBeliefFactory
    {
        private readonly GoapAgent _agent;
        private readonly Dictionary<string, GoapBelief> _beliefs;

        GoapBeliefFactory(GoapAgent agent, Dictionary<string, GoapBelief> beliefs)
        {
            _agent = agent;
            _beliefs = beliefs;
        }

        bool InRangeOf(Vector3 pos, float range) => Vector3.Distance(_agent.transform.position, pos) <= range;

        public void AddBelief(string key, Func<bool> condition)
        {
            _beliefs.Add(key, new GoapBelief.Builder(key)
                .WithCondition(condition)
                .Build());
        }
        public void AddLocationBelief(string key, float distance, Vector3 location)
        {
            _beliefs.Add(key, new GoapBelief.Builder(key)
                .WithCondition(() => InRangeOf(location, distance))
                .WithLocation(() => location)
                .Build()
            );
        }
        // public void AddSensorBelief(string key, Sensor sensor) {
        //     beliefs.Add(key, new AgentBelief.Builder(key)
        //         .WithCondition(() => sensor.IsTargetInRange)
        //         .WithLocation(() => sensor.TargetPosition)
        //         .Build());
        // }
    }
    

}
