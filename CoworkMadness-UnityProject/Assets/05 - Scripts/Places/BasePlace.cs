using AI_Motivation;
using Places.Process;
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
        public abstract float Neighbourhood { get; }

        // Registration 
        public abstract bool RegisterUser(GameObject user);
        public abstract bool UnregisterUser(GameObject user);
        
        // Process
        protected abstract IProcessStrategy ProcessStrategy { get; set; }
        // TODO : Encapsulate into strategy
        public abstract Status ProcessStatus { get; }
        public abstract void StartProcess();
        public abstract void Process(float deltaTime);
        public abstract void StopProcess();
        public abstract float ProcessProgress { get; }
        // Can abort ?
        public abstract bool CanAbort { get;}
        public abstract Transform EntryPoint { get; }
        
        private void Awake() => PlaceProvider = GetComponentInParent<PlaceProvider>();

        private void OnEnable() => PlaceProvider?.RegisterPlace(this);
        private void OnDisable() => PlaceProvider?.UnregisterPlace(this);
        
        // Attributes
        public abstract Attributes.Quality Quality { get;}
        
        


    }



}