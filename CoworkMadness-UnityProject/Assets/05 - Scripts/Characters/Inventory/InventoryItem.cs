using System;
using UnityEngine;
using UnityEngine.Serialization;

public class InventoryItem : MonoBehaviour
{
    [SerializeField] private bool isEquipped;
    
    private MeshRenderer _object;
    
    public bool IsEquipped
    {
        get => isEquipped;
        set
        {
            isEquipped = value;
            if(_object != null) _object.enabled = value;
        }
    }

    private void Start()
    {
        _object = GetComponent<MeshRenderer>();
        IsEquipped = false;
    }

}
