using System;
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
                Gizmos.color = Occupied ? Color.yellow : Color.green;
                Gizmos.DrawWireSphere(transform.position, 0.5f);
            }
        }
    }
}
