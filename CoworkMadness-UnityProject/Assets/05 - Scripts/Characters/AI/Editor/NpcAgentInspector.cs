using System.Linq;
using GOAP;
using UnityEditor;
using UnityEngine;
using AI;

[CustomEditor(typeof(NpcAgent))]
public class NpcAgentInspector : Editor
{
    public override void OnInspectorGUI()
    {
        
        if (Application.isPlaying)
            Repaint();
        
        GoapAgent agent = (GoapAgent)target;

        EditorGUILayout.Space();
        DrawDefaultInspector();

        // Show Goals
        // if (agent.CurrentGoal != null)
        // {
        //     EditorGUILayout.LabelField("Current Goal :", EditorStyles.boldLabel);
        //     EditorGUILayout.BeginHorizontal();
        //     GUILayout.Space(10);
        //     EditorGUILayout.LabelField(agent.CurrentGoal.Name);
        //     EditorGUILayout.EndHorizontal();
        // }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Goals :", EditorStyles.boldLabel);
        if (agent.Goals != null)
        {
            foreach (var goal in agent.Goals.OrderByDescending(g => g.Priority))
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(10);
                if (agent.CurrentGoal != null && goal == agent.CurrentGoal)
                    EditorGUILayout.LabelField(goal.Name + " : " + goal.Priority.ToString("P"), EditorStyles.boldLabel);
                else
                    EditorGUILayout.LabelField(goal.Name + " : " + goal.Priority.ToString("P"));
                EditorGUILayout.EndHorizontal();
            }
        }
        

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Actions :", EditorStyles.boldLabel); // Show current plan
        if (agent.CurrentAction != null)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(10);
            EditorGUILayout.LabelField(agent.CurrentAction.Name + " : " + (1.0f - agent.CurrentAction.Progress).ToString("P"),
                EditorStyles.boldLabel);
            EditorGUILayout.EndHorizontal();
        }

        if (agent.ActionPlan != null)
        {
            foreach (var a in agent.ActionPlan.Actions)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(10);
                EditorGUILayout.LabelField(a.Name);
                EditorGUILayout.EndHorizontal();
            }
        }


        // Show beliefs
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Beliefs :", EditorStyles.boldLabel);
        if (agent.Beliefs != null)
        {
            foreach (var belief in agent.Beliefs)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(10);
                EditorGUILayout.LabelField(belief.Key + ": " + belief.Value.Evaluate());
                EditorGUILayout.EndHorizontal();
            }
        }

    }
}
