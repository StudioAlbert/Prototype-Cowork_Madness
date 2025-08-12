using Unity.Behavior;

namespace AI_Motivation
{
    [BlackboardEnum]
    [System.Serializable]
    public enum GoalType
    {
        Idle,
        Work,
        Break,
        Social
    }
}
