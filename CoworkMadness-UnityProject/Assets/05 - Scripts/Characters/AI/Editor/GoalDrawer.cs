using System.Diagnostics.CodeAnalysis;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(AI_Motivation.Goal))]
[SuppressMessage("Usage", "DBG001:Debug.Log usage interdit")]
public class GoalDrawer : PropertyDrawer
{
    [SerializeField] private VisualTreeAsset _goalXML;

    public override VisualElement CreatePropertyGUI(SerializedProperty goalProperty)
    {
        // Create a new VisualElement to be the root the property UI.
        var container = new VisualElement();

        // Load the reference UXML.
        if (_goalXML == null)
            _goalXML = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/06 - UI/Character/AI/GoalProperty.uxml");

        // Instantiate the UXML.
        container = _goalXML.Instantiate();


        // Create drawer UI using C#.
       FloatField Priority = container.Q<FloatField>("Priority");
        Priority?.BindProperty(goalProperty.FindPropertyRelative("Priority"));

        EnumField status = container.Q<EnumField>("Type");
        status?.BindProperty(goalProperty.FindPropertyRelative("Type"));

        // var popup = new UnityEngine.UIElements.PopupWindow();
        // popup.text = "Goal";
        // popup.Add(new PropertyField(property.FindPropertyRelative("Name"), "Name"));
        // popup.Add(new PropertyField(property.FindPropertyRelative("Priority"), "Priority"));
        // container.Add(popup);

        // Return the finished UI.
        return container;
    }
}
