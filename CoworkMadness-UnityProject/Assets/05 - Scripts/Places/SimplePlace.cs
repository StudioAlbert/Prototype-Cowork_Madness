using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

// ReSharper disable once CheckNamespace
namespace Places
{
        public class SimplePlace : BasePlace
        {
            [SerializeField] private BasePlace.PlaceType type;
            [SerializeField] private bool available;


            public override bool Available
            {
                get => available;
                set => available = value;
            }
            public override BasePlace.PlaceType Type => type;
            public override Vector3 Position => transform.position;
            public override void SetProgress(float progress)
            {
                // Nope
            }

            public GameObject user;
        }
}
