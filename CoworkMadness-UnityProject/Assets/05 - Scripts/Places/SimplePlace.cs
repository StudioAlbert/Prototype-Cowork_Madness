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
            [SerializeField] private PlaceProvider _placeProvider;
            [SerializeField] private float _neighbourhood = 5f;

            public GameObject User => _user;

            protected override PlaceProvider PlaceProvider
            {
                get => _placeProvider; 
                set => _placeProvider = value;
            }
            public override bool Available =>  _user == null;
            public override GoalType Type => _type;
            public override Vector3 Position => transform.position;
            public override float Neighbourhood => _neighbourhood;

            public override bool RegisterUser(GameObject user)
            {
                _user = user;
                return true;
            }
            public override bool UnregisterUser(GameObject user)
            {
                _user = null;
                return true;
            }


        }
}
