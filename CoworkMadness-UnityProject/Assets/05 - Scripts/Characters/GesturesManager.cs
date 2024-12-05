using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GesturesManager : MonoBehaviour
{
    
    [SerializeField] PlayerInputController _inputController;

    [SerializeField] private Animator _animator;
    [SerializeField] [Range(0, 3)] private int _saluteIndex = 0;
    [SerializeField] private bool _isChating;

    [SerializeField] private float _minTempo = 0.4f;
    [SerializeField] private float _maxTempo = 0.7f;

    private Coroutine _playGestureCo = null;

    //private bool _saluteLock = false;

    private void Start()
    {
        
    }

    void OnEnable()
    {
        StartPlayGesture();
    }
    private void OnDisable()
    {
        StopPlayGesture();
    }

    // Update is called once per frame
    void Update()
    {
        if(_inputController.Salute)
        {
            Salute();
        }        
    }

    #region SALUTE
    public void Salute()
    {
        // Stop playing gestures
        StopPlayGesture();
        // Trigger Salute
        // pick a different salute
        _saluteIndex = Random.Range(0, 4);
        _animator.SetInteger(AnimatorHandles.SalutePosture, _saluteIndex);
        _animator.SetTrigger(AnimatorHandles.Salute);
        _animator.ResetTrigger(AnimatorHandles.Salute);

    }
    void OnSaluteEnd()
    {
        // TODO : Delete events in FBX Clips, without this function will happent a warning
        Debug.Log("Salute End");
    }
    void OnSaluteBegin()
    {
        // TODO : Delete events in FBX Clips, without this function will happent a warning
        Debug.Log("Salute Begin");
    }
    
    #endregion

    #region GESTURES

    private const int MaxGestures = 3;
    
    public void StartPlayGesture()
    {
        if (_playGestureCo == null)
        {
            _playGestureCo = StartCoroutine(PlayGesture());
        }
    }

    public void StopPlayGesture()
    {
        if (_playGestureCo != null)
        {
            StopCoroutine(_playGestureCo);
            _playGestureCo = null;
        }
            
    }

    private IEnumerator PlayGesture()
    {
        while (true)
        {
            //_animator.ResetTrigger("NewGesture");
            yield return new WaitForSeconds(Random.Range(_minTempo, _maxTempo));
            //_animator.SetInteger("GestureIndex", Random.Range(0,11));
            _animator.SetFloat(AnimatorHandles.GestureWeight, Random.Range(0f, 1f));
            _animator.SetTrigger(AnimatorHandles.NewGesture);
            yield return new WaitForEndOfFrame();
            _animator.ResetTrigger(AnimatorHandles.NewGesture);

        }
    }

    #endregion

}
