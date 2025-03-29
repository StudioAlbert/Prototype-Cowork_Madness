using UnityEngine;
using UnityEngine.InputSystem.Users;

// ReSharper disable once CheckNamespace
namespace Places
{
    public abstract class BasePlace : MonoBehaviour
    {
        public enum PlaceType
        {
            None,
            Work,
            Break,
            Social
        }
        
        public abstract bool Available { get; }
        
        public abstract PlaceType Type { get; }
        public abstract Vector3 Position { get; }
        public abstract void SetProgress(float progress);
    }


}