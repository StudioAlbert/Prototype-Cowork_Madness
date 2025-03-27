using System.Linq;
using GOAP;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GoapAgent))]
public class GoapAgentInspector : Editor
{
    public override void OnInspectorGUI()
    {
        GoapAgent agent = (GoapAgent)target;

        EditorGUILayout.Space();
        DrawDefaultInspector();

        EditorGUILayout.Space();

        // Show Goals
        EditorGUILayout.LabelField("Goals ordered:", EditorStyles.boldLabel);
        if (agent.Goals != null)
        {
            foreach (var goal in agent.Goals.OrderByDescending(g => g.Priority))
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(10);
                EditorGUILayout.LabelField(goal.Name + " : " + goal.Priority.ToString("P"));
                EditorGUILayout.EndHorizontal();
            }
        }

        if (agent.CurrentGoal != null)
        {
            EditorGUILayout.LabelField("Current Goal:", EditorStyles.boldLabel);
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(10);
            EditorGUILayout.LabelField(agent.CurrentGoal.Name);
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.Space();

        // Show current plan
        if (agent.ActionPlan != null)
        {
            EditorGUILayout.LabelField("Actions", EditorStyles.boldLabel);

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(10);
            EditorGUILayout.LabelField(agent.CurrentAction.Name + " : " + (1.0f - agent.CurrentAction.Progress).ToString("P"), EditorStyles.boldLabel);
            EditorGUILayout.EndHorizontal();

            foreach (var a in agent.ActionPlan.Actions)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(10);
                EditorGUILayout.LabelField(a.Name);
                EditorGUILayout.EndHorizontal();
            }
        }

        EditorGUILayout.Space();

        // Show beliefs
        EditorGUILayout.LabelField("Beliefs:", EditorStyles.boldLabel);
        if (agent.Beliefs != null)
        {
            foreach (var belief in agent.Beliefs)
            {
                if (belief.Key is "Nothing" or "Something") continue;
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(10);
                //EditorGUILayout.LabelField(belief.Key + ": " + belief.Value.Evaluate());
                EditorGUILayout.EndHorizontal();
            }
        }

        EditorGUILayout.Space();
    }
}
