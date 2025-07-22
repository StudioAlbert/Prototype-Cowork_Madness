using System;

namespace AI_Motivation
{
    [Serializable]
    class Goal
    {
        public GoalType Type { get; set; }
        public float Priority { get; set; }
        public Goal(GoalType type, float priority)
        {
            Type = type;
            Priority = priority;
        }
    }

}