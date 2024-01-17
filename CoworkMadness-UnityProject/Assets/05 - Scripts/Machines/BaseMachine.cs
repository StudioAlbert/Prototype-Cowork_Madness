using System;
using System.Collections;
using System.Collections.Generic;
using GOAPCore;
using UnityEngine;

public class BaseMachine : MonoBehaviour
{
    
    [SerializeField] private float _activationTime;
    [SerializeField] private GameObject _indicator;
    [SerializeField] private Transform _queuePosition; 
    
    private bool _isAvailable = true;
    public bool IsAvailable => _isAvailable;
    
    private bool _isActivated = true;
    public bool IsActivated => _isActivated;

    private GAgent _user;
    public GAgent User => _user;

    public Action OnTaskEnded;
    public Transform QueuePosition => _queuePosition;

    // Start is called before the first frame update
    void Start()
    {
        _queuePosition = transform;
        _isAvailable = true;
        _isActivated = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(_isAvailable)
            _indicator.GetComponent<MeshRenderer>().material.color = Color.green;
        else if(!_isActivated)
            _indicator.GetComponent<MeshRenderer>().material.color = Color.yellow;
        else
            _indicator.GetComponent<MeshRenderer>().material.color = Color.red;
    }

    public void Lock(GAgent user)
    {
        _isAvailable = false;
        _user = user;
    }
        
    public void BeginTask()
    {
        if(!_isActivated)
            StartCoroutine(CO_Task(_activationTime));
    }
    IEnumerator CO_Task(float time)
    {
        _isActivated = true;
        yield return new WaitForSeconds(time);
        _isActivated = false;
        _isAvailable = true;
        _user = null;
        
        OnTaskEnded.Invoke();
    }
}
