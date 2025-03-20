using System;
using UnityEngine;

public class Door : MonoBehaviour
{

    public int doorCandidates = 0;

    private Animator _doorAnimator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _doorAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        doorCandidates++;
        OpenTheDoor(true);
    }
    private void OnTriggerExit(Collider other)
    {
        doorCandidates--;
        if(doorCandidates == 0) OpenTheDoor(false);
    }
    private void OpenTheDoor(bool open)
    {
        _doorAnimator.SetBool("IsOpen", open);
    }
}
