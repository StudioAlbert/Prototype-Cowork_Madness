using UnityEditor;
using UnityEngine;

namespace Utilities
{
    
    [CustomEditor(typeof(LoggerObject))]
    public class LoggerEditor : Editor
    {

        public override void OnInspectorGUI()
        {

            LoggerObject lo = (LoggerObject) target;

            var msg = GUILayout.TextField(target.name);
            
            if (GUILayout.Button("Log"))
                lo.Log(msg);
            if (GUILayout.Button("Warning"))
                lo.Warning(msg);
            if (GUILayout.Button("Error"))
                lo.Error(msg);
            

        }

    }
}
