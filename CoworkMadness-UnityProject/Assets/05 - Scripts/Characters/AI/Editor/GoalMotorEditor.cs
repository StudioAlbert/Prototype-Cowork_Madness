using AI_Motivation;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(GoalMotor))]
public class GoalMotorEditor : Editor
{

    [SerializeField] private VisualTreeAsset _goalMotorXML;
    private VisualElement _myInspector;

    public override VisualElement CreateInspectorGUI()
    {
        _myInspector = new VisualElement();
        if (!_goalMotorXML)
            Debug.LogError("No Goal Motor XML");
        
        _myInspector = _goalMotorXML.Instantiate();
        
        // Get a reference to the default Inspector Foldout control.
        VisualElement InspectorFoldout = _myInspector.Q("DefaultInspector");
        if (InspectorFoldout != null)
        {
            // Attach a default Inspector to the Foldout.
            InspectorElement.FillDefaultInspector(InspectorFoldout, serializedObject, this);
        }
        
        return _myInspector;
    }
}
