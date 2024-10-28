using System;
using UnityEngine;

public class AnimatorHandles : MonoBehaviour
{
    static int _salute;
    static int _salutePosture;
    static int _newGesture;
    static int _gestureWeight;
    static int _isChatting;
    static int _walkSpeed;

    public static int Salute => _salute;
    public static int SalutePosture => _salutePosture;
    public static int NewGesture => _newGesture;
    public static int GestureWeight => _gestureWeight;
    public static int IsChatting => _isChatting;
    public static int WalkSpeed => _walkSpeed;
    
    private void Start()
    {
        _salute = Animator.StringToHash("Salute");
        _salutePosture = Animator.StringToHash("SalutePosture");
        _newGesture = Animator.StringToHash("NewGesture");
        _gestureWeight = Animator.StringToHash("GestureWeight");
        _isChatting = Animator.StringToHash("IsChatting");
        _walkSpeed = Animator.StringToHash("WalkSpeed");
    }

}
