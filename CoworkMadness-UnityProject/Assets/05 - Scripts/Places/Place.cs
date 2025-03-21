using UnityEngine;

public class Place : MonoBehaviour
{
    // [SerializeField] private Color _color = Color.yellow;
    [SerializeField] private PlaceType _type;
    
    public bool Available { get; private set; }
    public PlaceType Type => _type;
    public Vector3 Position => transform.position;

    // private void OnDrawGizmos()
    // {
    //     Gizmos.color = _color;
    //     Gizmos.DrawSphere(transform.position, .25f);
    // }
}
