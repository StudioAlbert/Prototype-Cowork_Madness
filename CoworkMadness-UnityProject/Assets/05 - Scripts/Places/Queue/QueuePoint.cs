using System;
using AI_Motivation;
using UnityEngine;

namespace Places
{
    namespace Queue
    {
        public class QueuePoint : MonoBehaviour
        {
            public bool Occupied;

            private void OnDrawGizmos()
            {
                Gizmos.color = Occupied ? new Color(1f,0.5f, 0): Color.green;
                Gizmos.DrawCube(transform.position + new Vector3(0,3,0), new Vector3(0.25f, 0.25f, 0.25f));
            }
        }
    }
}
