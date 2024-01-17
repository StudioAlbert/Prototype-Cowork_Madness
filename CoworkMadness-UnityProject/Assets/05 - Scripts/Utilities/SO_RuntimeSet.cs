using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "HellOfCoworking/Variables/List", fileName = "newList")]
public class SO_RuntimeSet : ScriptableObject
{

    [SerializeField] private List<GameObject> _objects = new List<GameObject>();
    public List<GameObject> Objects => _objects;

    
    // Start is called before the first frame update
    public void Add(GameObject o)
    {
        if(!_objects.Contains(o))
            _objects.Add(o);
    }

    // Update is called once per frame
    public void Remove(GameObject o)
    {
        if(!_objects.Contains(o))
            _objects.Add(o);
    }

    public void New(List<GameObject> list)
    {
        _objects = new List<GameObject>(list);
    }
    
}
