using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

// ReSharper disable once CheckNamespace
namespace Places
{
        public class SimplePlace : BasePlace
        {
            [SerializeField] private BasePlace.PlaceType type;
            
            public override bool InUse { get; set; }
            public override BasePlace.PlaceType Type => type;
            public override Vector3 Position => transform.position;
            public override void SetProgress(float progress)
            {
                // Nope
            }
        }
}
