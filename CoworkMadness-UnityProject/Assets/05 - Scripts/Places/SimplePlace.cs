using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

// ReSharper disable once CheckNamespace
namespace Places
{
        public class SimplePlace : BasePlace
        {
            [SerializeField] private PlaceType _type;
            [SerializeField] private GameObject _user;
            
            public override bool Available =>  _user == null;
            public override PlaceType Type => _type;
            public override Vector3 Position => transform.position;
            public GameObject User => _user;
            
            public void RegisterUser(GameObject user) => _user = user;
            public void UnregisterUser(GameObject user) => _user = null;
            
            
            
        }
}
