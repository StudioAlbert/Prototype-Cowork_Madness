using System;
using System.Collections.Generic;
using AI_Motivation;
using GOAP;
using Places;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(GoapAgent))]
public class MoodGoap : MonoBehaviour
{

    [Header("Erosion rates")]
    [SerializeField] private float _workOverTime;
    [SerializeField] private float _breakOverTime;
    [SerializeField] private float _socialOverTime;

    private HashSet<GoapGoal> _goals;
    private GoapAgent _agent;

    private void Awake() => _agent = GetComponent<GoapAgent>();
    void OnEnable() => _agent.OnGoalSucceed += OnAgentGoalDone;
    void OnDisable() => _agent.OnGoalSucceed -= OnAgentGoalDone;
    void Update()
    {
        UpdateGoals(Time.deltaTime);
    }

    private void OnAgentGoalDone(GoapGoal goal)
    {
        goal.ResetPriority();
    }

    void UpdateGoals(float deltaTime)
    {
        if (_agent.Goals == null || _agent.Goals.Count <= 0)
            return;

        foreach (var goal in _agent.Goals)
            UpdateOneGoal(goal, deltaTime);

    }
    private void UpdateOneGoal(GoapGoal goal, float deltaTime)
    {

        switch (goal.GoalType)
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
}
