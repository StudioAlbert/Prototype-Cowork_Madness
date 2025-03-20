using System.Collections.Generic;
using System.Data.SqlTypes;
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
        
        public string Name { get; }
        public float Priority { get; set; }
        public PlaceType PlaceType { get; private set; }
        public HashSet<GoapBelief> DesiredEfffects = new();
        
        public class Builder
        {

            private readonly GoapGoal _goal;
            
            Builder(string name)
            {
                _goal = new GoapGoal(name);
            }

            Builder WithPriority(float priority)
            {
                _goal.Priority = priority;
                return this;
            }
            Builder WithType(PlaceType type)
            {
                _goal.PlaceType = type;
                return this;
            }
            Builder AddDesiredEffect(GoapBelief effect)
            {
                _goal.DesiredEfffects.Add(effect);
                return this;
            }
            GoapGoal Build()
            {
                return _goal;
            }
            
        }

    }
}
