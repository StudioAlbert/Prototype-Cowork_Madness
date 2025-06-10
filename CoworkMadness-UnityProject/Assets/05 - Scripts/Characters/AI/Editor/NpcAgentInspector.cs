#define UI_TOOLKIT_EDITOR

using System.Diagnostics.CodeAnalysis;
using System.Linq;
using UnityEditor;
using UnityEngine;
using AI;
using UnityEditor.UIElements;
using UnityEngine.Serialization;
using UnityEngine.UIElements;



[CustomEditor(typeof(NpcAgent))]
[SuppressMessage("Usage", "DBG001:Debug.Log usage interdit")]
public class NpcAgentInspector : Editor
{
    [SerializeField] private VisualTreeAsset _mNpcAgentXML;

    private TextField _currentGoalText;

#if !UI_TOOLKIT_EDITOR
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
#else
    // Create a new VisualElement to be the root of the Inspector UI.
    private VisualElement _myInspector;
    
    public override VisualElement CreateInspectorGUI()
    {
    
        _myInspector = new VisualElement();
        
        // Load the reference UXML.
        if (_mNpcAgentXML == null)
            _mNpcAgentXML = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/06 - UI/Character/AI/NPCAgentEditor.uxml");
    
        // Instantiate the UXML.
        _myInspector = _mNpcAgentXML.Instantiate();
        
        // Track when serialized object changes
        //_myInspector.TrackSerializedObjectValue(serializedObject, OnSerializedObjectChanged);
    
        // Get a reference to the default Inspector Foldout control.
        VisualElement InspectorFoldout = _myInspector.Q("DefaultInspector");
        if (InspectorFoldout != null)
        {
            // Attach a default Inspector to the Foldout.
            InspectorElement.FillDefaultInspector(InspectorFoldout, serializedObject, this);
        }
    
        // Return the finished Inspector UI.
        return _myInspector;
    }

    // private IntegerField _currentGoalField;
    // private void OnSerializedObjectChanged(SerializedObject obj)
    // {
    //     // Update UI when properties change
    //     Debug.Log("Serialized object changed!");
    //     
    //     _currentGoalField = _myInspector.Q<IntegerField>("CurrentGoal");
    //     SerializedProperty currentGoal = obj.FindProperty("_currentGoal");
    //     if (_currentGoalField != null && currentGoal != null)
    //     {
    //         _currentGoalField.label = currentGoal.FindPropertyRelative("_name").stringValue;
    //         _currentGoalField.value = currentGoal.FindPropertyRelative("_priority").intValue;
    //     }
    //     
    //     if (Application.isPlaying)
    //         Repaint();
    //     
    // }
    
    
#endif
}
