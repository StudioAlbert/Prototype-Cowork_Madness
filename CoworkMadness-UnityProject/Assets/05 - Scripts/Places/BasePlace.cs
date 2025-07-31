using AI_Motivation;
using UnityEngine;
using UnityEngine.InputSystem.Users;

// ReSharper disable once CheckNamespace
namespace Places
{
    public abstract class BasePlace : MonoBehaviour
    {
        protected abstract PlaceProvider PlaceProvider { get; set; }
        
        public abstract bool Available { get; }
        public abstract GoalType Type { get; }
        public abstract Vector3 Position { get; }
        public abstract float Neighbourhood { get; }

        public abstract bool RegisterUser(GameObject user);
        public abstract bool UnregisterUser(GameObject user);
        
        
        private void Awake() => PlaceProvider = GetComponentInParent<PlaceProvider>();

        private void OnEnable() => PlaceProvider?.RegisterPlace(this);
        private void OnDisable() => PlaceProvider?.UnregisterPlace(this);
        
    }



}