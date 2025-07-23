using AI_Motivation;
using UnityEngine;
using UnityEngine.InputSystem.Users;

// ReSharper disable once CheckNamespace
namespace Places
{
    public abstract class BasePlace : MonoBehaviour
    {
        private PlaceProvider _placeProvider;
        
        public abstract bool Available { get; }
        public abstract GoalType Type { get; }
        public abstract Vector3 Position { get; }
        public abstract GameObject User { get; }
        
        public virtual void RegisterUser(GameObject user) { }
        public virtual void UnregisterUser(GameObject user) { }
        
        
        private void Awake() => _placeProvider = GetComponentInParent<PlaceProvider>();

        private void OnEnable() => _placeProvider?.RegisterPlace(this);
        private void OnDisable() => _placeProvider?.UnregisterPlace(this);
        
    }


}