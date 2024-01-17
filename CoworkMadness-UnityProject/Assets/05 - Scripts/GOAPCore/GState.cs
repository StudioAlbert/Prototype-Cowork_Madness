using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GState
{
    [SerializeField] private string _hash;
    public string Hash => _hash;

    [SerializeField] private int _count;
    public int Count { get => _count; set => _count = value; }

    // TO DO : Conditional state
    
    public GState(string hash, int value)
    {
        _hash = hash;
        _count = value;
    }
}
