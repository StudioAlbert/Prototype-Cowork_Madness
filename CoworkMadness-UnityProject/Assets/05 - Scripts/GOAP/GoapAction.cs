using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using Utilities;

namespace GOAP
{
    [Serializable]
    public class GoapAction : HasGoapStatus
    {

        GoapAction(string name)
        {
            _name = name;
        }

        [SerializeField] private string _name;
        public string Name => _name;

        [SerializeField] private float _cost;
        public float Cost => _cost;

        [SerializeField] private float _progress;

        private readonly HashSet<GoapBelief> _preconditions = new HashSet<GoapBelief>();
        private readonly HashSet<GoapBelief> _postConditions = new HashSet<GoapBelief>();
        private readonly HashSet<Action> _consequences = new HashSet<Action>();

        public HashSet<GoapBelief> Preconditions => _preconditions;
        public HashSet<GoapBelief> PostConditions => _postConditions;

        private IGoapActionStrategy _strategy;


        // Copy cat the strategy, Start, Update, stop
        public void Start()
        {
            _progress = 1;
            _strategy.Start();
            _status = GoapStatus.InProgress;
        }
        public void Stop()
        {
            _progress = 0;
            _strategy.Stop();
            _status = GoapStatus.Invalid;
        }

        public void Update(float deltaTime)
        {

            // If we can apply the strategy, then update it
            if (_strategy.CanPerform)
                _strategy.Update(deltaTime);

            _progress = _strategy.Progress;

            // If strategy is done, apply effects
            // If not, end it
            if (_strategy.Complete || _strategy.Failed)
            {
                foreach (Action c in _consequences)
                {
                    c.Invoke();
                }
            }
            if (_strategy.Complete) _status = GoapStatus.Complete;
            if (_strategy.Failed) _status = GoapStatus.Failed;

        }


        // BUILDER 
        public class Builder
        {
            private readonly GoapAction _action;

            public Builder(string name)
            {
                _action = new GoapAction(name)
                {
                    _cost = 1f
                };
            }

            public Builder WithCost(float cost)
            {
                _action._cost = cost;
                return this;
            }
            public Builder WithStrategy(IGoapActionStrategy strategy)
            {
                _action._strategy = strategy;
                return this;
            }
            public Builder AddPrecondition(GoapBelief condition)
            {
                _action._preconditions.Add(condition);
                return this;
            }
            public Builder AddPostCondition(GoapBelief condition)
            {
                _action._postConditions.Add(condition);
                return this;
            }
            public Builder AddConsequence(Action consequence)
            {
                _action._consequences.Add(consequence);
                return this;
            }
            public GoapAction Build()
            {
                return _action;
            }
        }

    }
}
