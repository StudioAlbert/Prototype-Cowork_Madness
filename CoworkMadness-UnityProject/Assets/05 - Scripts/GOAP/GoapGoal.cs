using System;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.Properties;
using UnityEditor;
using UnityEngine;
using Utilities;

namespace GOAP
{
    [Serializable]
    public class GoapGoal : HasGoapStatus
    {
        GoapGoal(string name)
        {
            _name = name;
        }

        private float _startPriority;

        [SerializeField] private string _name;
        public string Name => _name;

        [SerializeField] private float _priority;
        public float Priority
        {
            get => _priority;
            set => _priority = value;
        }

        public Places.PlaceType PlaceType { get; private set; }
        public readonly HashSet<GoapBelief> DesiredEffects = new HashSet<GoapBelief>();
        
        public void SetInProgress() => _status = GoapStatus.InProgress;
        
        public class Builder
        {

            private readonly GoapGoal _goal;
            
            public Builder(string name)
            {
                _goal = new GoapGoal(name);
            }

            public Builder WithPriority(float priority)
            {
                _goal._startPriority = priority;
                _goal.Priority = priority;
                return this;
            }
            public Builder WithType(Places.PlaceType type)
            {
                _goal.PlaceType = type;
                return this;
            }
            public Builder WithDesiredEffect(GoapBelief effect)
            {
                _goal.DesiredEffects.Add(effect);
                return this;
            }
            public GoapGoal Build()
            {
                return _goal;
            }
            
        }

        public void ResetPriority()
        {
            Priority = _startPriority;
        }
    }
}
