using UnityEngine;

public class Morale : MonoBehaviour
{
    
    public float _moraleScore;
    
    public float MoraleScore => _moraleScore;

    public void UpdateMoraleAmongGoals(GoalType type, bool successful)
    {
        switch (type)
        {
            case GoalType.Work:
                _moraleScore += (successful ? -1 : 0);
                break;
            case GoalType.Break:
                _moraleScore += (successful ? 3 : -6);
                break;
            case GoalType.Social:
                _moraleScore += (successful ? 8 : -4);
                break;
            case GoalType.Idle:
                break;
        }
    }
    
}
