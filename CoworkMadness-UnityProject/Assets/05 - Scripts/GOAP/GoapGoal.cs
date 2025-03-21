using System.Collections.Generic;

namespace GOAP
{
    public class GoapGoal
    {
        GoapGoal(string name)
        {
            Name = name;
        }
        
        public string Name { get; }
        public float Priority { get; set; }
        public PlaceType PlaceType { get; private set; }
        public HashSet<GoapBelief> DesiredEfffects = new();
        
        public class Builder
        {

            private readonly GoapGoal _goal;
            
            public Builder(string name)
            {
                _goal = new GoapGoal(name);
            }

            public Builder WithPriority(float priority)
            {
                _goal.Priority = priority;
                return this;
            }
            public Builder WithType(PlaceType type)
            {
                _goal.PlaceType = type;
                return this;
            }
            public Builder WithDesiredEffect(GoapBelief effect)
            {
                _goal.DesiredEfffects.Add(effect);
                return this;
            }
            public GoapGoal Build()
            {
                return _goal;
            }
            
        }

    }
}
