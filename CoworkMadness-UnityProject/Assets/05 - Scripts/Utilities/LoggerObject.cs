using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace Utilities
{
    
    [SuppressMessage("Usage", "DBG001:Debug.Log usage interdit")]
    public class LoggerObject : ILogger
    {

        // overrides
        public override void Active(bool active) => enabled = active;
        
        public string Prefix { get; set; }
        private string Message(string content) => $"[{Prefix}] : {content}";
        
        private void Start() => Prefix = gameObject.name;

        public void Log(string content)
        {
            if (enabled) Debug.Log(Message(content));
        }
        public void Warning(string content)
        {
            if(enabled) Debug.LogWarning(Message(content));
        }
        public void Error(string content)
        {
            if(enabled) Debug.LogError(Message(content));
        }

        
    }

}
