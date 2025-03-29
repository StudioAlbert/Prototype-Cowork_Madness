using System.Collections.Generic;
using Places;
using UnityEditor;

namespace GOAP
{
    public class GoapGoal
    {
        GoapGoal(string name)
        {
            Name = name;
        }

        private float _startPriority;
        
        public string Name { get; }
        public float Priority { get; set; }
        public BasePlace.PlaceType PlaceType { get; private set; }
        public readonly HashSet<GoapBelief> DesiredEffects = new HashSet<GoapBelief>();
        
        public class Builder
        {

            private readonly GoapGoal _goal;
            
            public Builder(string name)
            {
                _goal = new GoapGoal(name);
            }

            public Builder WithPriority(float priority)
            {
                _goal._startPriority = priority;
                _goal.Priority = priority;
                return this;
            }
            public Builder WithType(BasePlace.PlaceType type)
            {
                _goal.PlaceType = type;
                return this;
            }
            public Builder WithDesiredEffect(GoapBelief effect)
            {
                _goal.DesiredEffects.Add(effect);
                return this;
            }
            public GoapGoal Build()
            {
                return _goal;
            }
            
        }

        public void ResetPriority()
        {
            Priority = _startPriority;
        }
    }
}
