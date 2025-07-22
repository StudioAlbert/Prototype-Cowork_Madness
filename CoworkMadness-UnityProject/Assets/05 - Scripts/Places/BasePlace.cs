using UnityEngine;
using UnityEngine.InputSystem.Users;

// ReSharper disable once CheckNamespace
namespace Places
{
    public enum PlaceType
    {
        None,
        Work,
        Break,
        Social
    }
    
    public abstract class BasePlace : MonoBehaviour
    {
        public abstract bool Available { get; }
        public abstract PlaceType Type { get; }
        public abstract Vector3 Position { get; }
    }


}