using System;

[Serializable]
class Mood
{
    public GoalType Type { get; set; }
    public float Priority { get; set; }
    public Mood(GoalType type, float priority)
    {
        Type = type;
        Priority = priority;
    }
}
