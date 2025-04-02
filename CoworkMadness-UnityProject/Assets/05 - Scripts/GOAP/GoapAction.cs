using System;
using System.Collections.Generic;
using UnityEditor;

namespace GOAP
{
    public class GoapAction
    {

        GoapAction(string name)
        {
            Name = name;
        }

        public string Name { get; }
        public float Cost { get; private set; }

        private readonly HashSet<GoapBelief> _preconditions = new HashSet<GoapBelief>();
        private readonly HashSet<GoapBelief> _postConditions = new HashSet<GoapBelief>();
        private readonly HashSet<Action> _consequences = new HashSet<Action>();

        public HashSet<GoapBelief> Preconditions => _preconditions;
        public HashSet<GoapBelief> PostConditions => _postConditions;

        private IGoapActionStrategy _strategy;
        public bool Complete => _strategy.Complete;
        public bool Failed => _strategy.Failed;
        public float Progress => _strategy.Progress;

        // Copy cat the strategy, Start, Update, stop
        public void Start() => _strategy.Start();
        public void Stop() => _strategy.Stop();

        public void Update(float deltaTime)
        {
            // If we can apply the strategy , then update it
            if (_strategy.CanPerform)
                _strategy.Update(deltaTime);

            // If strategy is done, apply effects
            // If not, end it
            if (_strategy.Complete)
            {
                foreach (Action c in _consequences)
                {
                    c.Invoke();
                }
            }
            
        }

        // BUILDER 
        public class Builder
        {
            private readonly GoapAction _action;

            public Builder(string name)
            {
                _action = new GoapAction(name)
                {
                    Cost = 1f
                };
            }

            public Builder WithCost(float cost)
            {
                _action.Cost = cost;
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
