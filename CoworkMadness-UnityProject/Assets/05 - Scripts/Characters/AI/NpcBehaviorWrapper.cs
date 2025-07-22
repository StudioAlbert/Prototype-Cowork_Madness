using AI_Motivation;
using Unity.Behavior;
using UnityEngine;

public class NpcBehaviorWrapper : MonoBehaviour
{
    [SerializeField] private GoalMotor _motor;
    [SerializeField] private Blackboard _blackboard;
    
    private BlackboardVariable _goalType;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _blackboard = GetComponent<Blackboard>();
        _goalType = _blackboard.Variables.Find(v => v.Name == "GoalType");
    }

    // Update is called once per frame
    void Update()
    {
        if(_goalType != null)
        {
            _goalType.ObjectValue = _motor.BestGoalType;
        }
    }
}
