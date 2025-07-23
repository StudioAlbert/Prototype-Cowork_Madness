using UnityEngine;

namespace AI_Motivation
{

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
                    _moraleScore += (successful ? 10 : -5);
                    break;
                case GoalType.Social:
                    _moraleScore += (successful ? 15 : -7.5f);
                    break;
                case GoalType.Idle:
                    break;
            }
            
            // Debug.Log($"UPDATE MORALE :: [{gameObject.name}] : [{type}] ? {successful} => ({_moraleScore})");
            
        }

    }
}
