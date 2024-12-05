using System;
using UnityEngine;

public class AnimatorHandles
{
    static int _salute = 0;
    static int _salutePosture = 0;
    static int _newGesture = 0;
    static int _gestureWeight = 0;
    static int _isChatting = 0;
    static int _walkSpeed = 0;

    public static int Salute
    {
        get
        {
            if (_salute == 0) _salute = StringToHash("Salute");
            return _salute;
        }
    }

    public static int SalutePosture
    {
        get
        {
            if (_salutePosture == 0) _salutePosture = StringToHash("SalutePosture");
            return _salutePosture;
        }
    }

    public static int NewGesture
    {
        get
        {
            if (_newGesture == 0) _newGesture = StringToHash("NewGesture");
            return _newGesture;
        }
    }

    public static int GestureWeight
    {
        get
        {
            if (_gestureWeight == 0) _gestureWeight = StringToHash("GestureWeight");
            return _gestureWeight;
        }
    }

    public static int IsChatting
    {
        get
        {
            if (_isChatting == 0) _isChatting = StringToHash("IsChatting");
            return _isChatting;
        }
    }

    public static int WalkSpeed
    {
        get
        {
            if (_walkSpeed == 0) _walkSpeed = StringToHash("WalkSpeed");
            return _walkSpeed;
        }
    }

    private static int StringToHash(string stringToHash)
    {
        int hash = Animator.StringToHash(stringToHash);
        if (hash == 0)
        {
            Debug.LogWarning("Hash [" + stringToHash + "] is empty");
        }
        return hash;
    }
}
