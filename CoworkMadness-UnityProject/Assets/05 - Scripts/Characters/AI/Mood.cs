using System;
using System.Collections.Generic;
using GOAP;
using Places;
using UnityEngine;

[RequireComponent(typeof(GoapAgent))]
public class Mood : MonoBehaviour
{

    [Header("Erosion rates")]
    [SerializeField] private float workOverTime;
    [SerializeField] private float breakOverTime;
    [SerializeField] private float socialOverTime;
    
    private HashSet<GoapGoal> _goals;
    private GoapAgent _agent;

    private void Awake() => _agent = GetComponent<GoapAgent>();
    void OnEnable() => _agent.OnGoalDone += OnAgentGoalDone;
    void OnDisable() => _agent.OnGoalDone -= OnAgentGoalDone;
    void Update()
    {
        UpdateGoals(Time.deltaTime);
    }

    private void OnAgentGoalDone(GoapGoal goal)
    {
        goal.ResetPriority();
    }
    
    void UpdateCurrentGoal(float deltaTime)
    {
        
        if(_agent.CurrentGoal == null)
            return;
        
       UpdateOneGoal(_agent.CurrentGoal, deltaTime);


    }
    void UpdateGoals(float deltaTime)
    {
        
        if(_agent.Goals == null || _agent.Goals.Count <= 0)
            return;

        foreach (var goal in _agent.Goals)
        {
            UpdateOneGoal(goal, deltaTime);
        }
        


    }
    private void UpdateOneGoal(GoapGoal goal, float deltaTime)
    {

        switch (goal.PlaceType)
        {
            case BasePlace.PlaceType.Work:
                goal.Priority += workOverTime * deltaTime / 100.0f;
                break;
            case BasePlace.PlaceType.Break:
                goal.Priority += breakOverTime * deltaTime / 100.0f;
                break;
            case BasePlace.PlaceType.Social:
                goal.Priority += socialOverTime * deltaTime / 100.0f;
                break;
            case BasePlace.PlaceType.None:
                // No updates, stay the same
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        //goal.Priority = Mathf.Clamp(goal.Priority, 0, 1);
    }
}
