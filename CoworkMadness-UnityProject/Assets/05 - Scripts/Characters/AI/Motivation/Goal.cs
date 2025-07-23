using System;

namespace AI_Motivation
{
    [Serializable]
    public class Goal
    {
        public GoalType Type;
        public float Priority;
        public Goal(GoalType type, float priority)
        {
            Type = type;
            Priority = priority;
        }
    }

}