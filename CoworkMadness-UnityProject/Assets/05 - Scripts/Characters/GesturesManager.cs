using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GesturesManager : MonoBehaviour
{
    
    [SerializeField] private Animator _animator;
    private int _gestureIndex = 0;
    
    Coroutine _playGestureCO;


    private void Start()
    {
        // _animator = GetComponent<Animator>();
    }

    void OnEnable()
    {
        _playGestureCO = StartCoroutine(PlayGesture());
    }
    private void OnDisable()
    {
        StopCoroutine(_playGestureCO);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private IEnumerator PlayGesture()
    {
        while (true)
        {
            _animator.SetInteger("GestureIndex", _gestureIndex);
            _gestureIndex++;
            yield return new WaitForSeconds(1);
        }
    }
}
