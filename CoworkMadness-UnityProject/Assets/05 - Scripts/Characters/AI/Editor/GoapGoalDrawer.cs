using System;
using System.Diagnostics.CodeAnalysis;
using GOAP;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(GoapGoal))]
[SuppressMessage("Usage", "DBG001:Debug.Log usage interdit")]
public class GoapGoalDrawer : PropertyDrawer
{
    [SerializeField] private VisualTreeAsset _goalXML;

    public override VisualElement CreatePropertyGUI(SerializedProperty goalProperty)
    {
        // Create a new VisualElement to be the root the property UI.
        var container = new VisualElement();

        // Load the reference UXML.
        if (_goalXML == null)
            _goalXML = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/06 - UI/Character/AI/NPCAgentEditor.uxml");

        // Instantiate the UXML.
        container = _goalXML.Instantiate();


        // Create drawer UI using C#.
        Label nameLabel = container.Q<Label>("Name");
        SerializedProperty p = goalProperty.FindPropertyRelative("_name");
        if (nameLabel != null && p != null)
        {
            nameLabel.BindProperty(p);
        }
        else
        {
            Debug.LogError($"Binding situation : {nameLabel} {p?.name} {goalProperty}");
        }

        FloatField Priority = container.Q<FloatField>("Priority");
        Priority?.BindProperty(goalProperty.FindPropertyRelative("_priority"));


        // var popup = new UnityEngine.UIElements.PopupWindow();
        // popup.text = "Goal";
        // popup.Add(new PropertyField(property.FindPropertyRelative("Name"), "Name"));
        // popup.Add(new PropertyField(property.FindPropertyRelative("Priority"), "Priority"));
        // container.Add(popup);

        // Return the finished UI.
        return container;
    }
}
