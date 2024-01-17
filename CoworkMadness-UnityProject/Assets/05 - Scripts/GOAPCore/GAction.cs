using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace GOAPCore
{
    public abstract class GAction : MonoBehaviour
    {

        // [SerializeField] private GameObject _target;
        [SerializeField] private string _actionName = "defaultAction";
        [SerializeField] private int _basicCost = 0;
        [SerializeField][Tooltip("Duration (in seconds")] private float _stopDuration;
        [SerializeField] private List<GState> _preConditions = new List<GState>();
        [SerializeField] private List<GState> _postEffects = new List<GState>();
    
        // Maybe Agent properties ---------------------------------------------------
        protected GAgent _gAgent;
        private GWorld _gWorld;

        public Action ActionHasEnded;
        
        public string ActionName => _actionName;
        public List<GState> PreConditions => _preConditions;
        protected void Awake()
        {
            
            var gWorlds = FindObjectsByType<GWorld>(FindObjectsSortMode.InstanceID);
            if(gWorlds.Length > 1)
                Debug.LogError("Multiple GOAP World !");
            if(gWorlds.Length <= 0)
                Debug.LogError("NO GOAP World !");

            _gWorld = gWorlds[0];
            
        }
    
        public void RegisterAgent(GAgent gAgent)
        {
            _gAgent = gAgent;
        }
        
        public int Cost()
        {
            return _basicCost;
        }

        public bool IsAchievable()
        {
            return true;
        }

        public virtual bool IsAchievableGiven(List<GState> conditions)
        {

            foreach (GState condition in conditions)
            {
                if (!_postEffects.Exists(p => p.Hash == condition.Hash))
                    return false;
            }
            return true;
            
        }

        public virtual bool CanPerform()
        {
            foreach (GState condition in _preConditions)
            {
                if (!_gWorld.States.Exists(p => p.Hash == condition.Hash))
                {
                    Debug.Log(ActionName + " : This State is not fullfilled." + condition.Hash);
                    return false;
                }
            }
            return true;
        }

        public virtual void PrePerform()
        {
            Debug.Log(_gAgent.AgentName + ":" + ActionName + " started !");
        }
        public virtual void PostPerform()
        {
            Debug.Log(_gAgent.AgentName + ":" + ActionName+ " ended !");
            foreach (GState effect in _postEffects)
            {
                _gWorld.SetState(effect.Hash, effect.Count);
            }
            // Drop the end Event
            ActionHasEnded?.Invoke();
        }
        public abstract void Perform();

    }
}
