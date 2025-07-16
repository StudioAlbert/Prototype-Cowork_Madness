using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

// ReSharper disable once CheckNamespace
namespace Places
{
        public class SimplePlace : BasePlace
        {
            [SerializeField] private PlaceType _type;
            [SerializeField] private bool _available;


            public override bool Available
            {
                get => _available;
                set => _available = value;
            }
            public override PlaceType Type => _type;
            public override Vector3 Position => transform.position;

            public GameObject _user;
        }
}
