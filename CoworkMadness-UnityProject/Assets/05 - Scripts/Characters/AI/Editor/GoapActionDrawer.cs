using GOAP;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(GoapAction))]
public class GoapActionDrawer : PropertyDrawer
{
   [SerializeField] private VisualTreeAsset _treeAsset;
    
    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        // Create a new VisualElement to be the root the property UI.
        var container = new VisualElement();
        
        // Load the reference UXML.
        if (_treeAsset == null)
            _treeAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/06 - UI/Character/AI/NPCAgentEditor.uxml");

        // Instantiate the UXML.
        container = _treeAsset.Instantiate();
        
        // Create drawer UI using C#.
        Label nameLabel = container.Q<Label>("Name");
        nameLabel?.BindProperty(property.FindPropertyRelative("_name"));
        
        FloatField priority = container.Q<FloatField>("Progress");
        priority?.BindProperty(property.FindPropertyRelative("_progress"));
        
        EnumField status = container.Q<EnumField>("Status");
        status?.BindProperty(property.FindPropertyRelative("_status"));
        
        // var popup = new UnityEngine.UIElements.PopupWindow();
        // popup.text = "Goal";
        // popup.Add(new PropertyField(property.FindPropertyRelative("Name"), "Name"));
        // popup.Add(new PropertyField(property.FindPropertyRelative("Priority"), "Priority"));
        // container.Add(popup);

        // Return the finished UI.
        return container;
    }
}
