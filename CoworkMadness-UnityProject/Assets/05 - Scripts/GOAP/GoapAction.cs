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

        private HashSet<GoapBelief> _preconditions = new();
        private HashSet<GoapBelief> _effects = new();

        private IActionStrategy _strategy;
        public bool Complete => _strategy.Complete;
        
        // Copy cat the strategy, Start, Update, stop
        public void Start() => _strategy.Start();
        public void Stop() => _strategy.Stop();

        public void Update(float deltaTime)
        {
            // If we can apply the strategy , then update it
            if(_strategy.CanPerform)
                _strategy.Update(deltaTime);

            // If strategy is done, apply effects
            // If not, end it
            if (_strategy.Complete)
            {
                foreach (GoapBelief effect in _effects)
                {
                    effect.Evaluate();
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
            public Builder WithStrategy(IActionStrategy strategy)
            {
                _action._strategy = strategy;
                return this;
            }
            public Builder AddPrecondition(GoapBelief condition)
            {
                _action._preconditions.Add(condition);
                return this;
            }
            public Builder AddEffect(GoapBelief effect)
            {
                _action._effects.Add(effect);
                return this;
            }
            public GoapAction Build()
            {
                return _action;
            }
        }

    }
}
