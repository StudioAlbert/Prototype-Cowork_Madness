using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using AI_Motivation;


namespace AI_Motivation
{
    public class GoalMotor : MonoBehaviour
    {

        public GoalType BestGoalType { get; set; }

        [Header("Erosion rates")]
        [SerializeField] private float _workOverTime;
        [SerializeField] private float _breakOverTime;
        [SerializeField] private float _socialOverTime;

        [SerializeField] private List<Goal> _moods = new List<Goal>();


        void Awake()
        {
            _moods.Add(new Goal(GoalType.Idle, 1));
            _moods.Add(new Goal(GoalType.Work, 1));
            _moods.Add(new Goal(GoalType.Break, 1));
            _moods.Add(new Goal(GoalType.Social, 1));
        }

        void Update()
        {
            UpdateGoals(Time.deltaTime);
            if (_moods.Count > 0)
            {
                BestGoalType = _moods.OrderByDescending(g => g.Priority).First().Type;
            }
            else
            {
                BestGoalType = GoalType.Idle;
            }
        }

        // private void OnAgentGoalDone(float goal)
        // {
        //     goal.ResetPriority();
        // }

        void UpdateGoals(float deltaTime)
        {
            foreach (Goal mood in _moods)
            {
                UpdateOneGoal(mood, deltaTime);
            }
        }
        private void UpdateOneGoal(Goal goal, float deltaTime)
        {

            switch (goal.Type)
            {
                case GoalType.Work:
                    goal.Priority += _workOverTime * deltaTime / 100.0f;
                    break;
                case GoalType.Break:
                    goal.Priority += _breakOverTime * deltaTime / 100.0f;
                    break;
                case GoalType.Social:
                    goal.Priority += _socialOverTime * deltaTime / 100.0f;
                    break;
                case GoalType.Idle:
                    // No updates, stay the same
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            //goal.Priority = Mathf.Clamp(goal.Priority, 0, 1);
        }
        public void ResetPriority(GoalType type, float newPriority)
        {
            var goals = _moods.Where(g => g.Type == type);
            foreach (Goal goal in goals)
            {
                goal.Priority = newPriority;
            }
        }
    }
}
