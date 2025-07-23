using System;
using AI_Motivation;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

// ReSharper disable once CheckNamespace
namespace Places
{
        public class SimplePlace : BasePlace
        {
            [SerializeField] private GoalType _type;
            [SerializeField] private GameObject _user;
            
            private PlaceProvider _placeProvider;
            
            public override bool Available =>  _user == null;
            public override GoalType Type => _type;
            public override Vector3 Position => transform.position;
            public override GameObject User => _user;
            
            public override void RegisterUser(GameObject user) => _user = user;
            public override void UnregisterUser(GameObject user) => _user = null;


        }
}
