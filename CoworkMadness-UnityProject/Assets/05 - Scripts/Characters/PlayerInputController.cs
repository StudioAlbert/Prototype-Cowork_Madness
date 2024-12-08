using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    private float _rotation;
    public float Rotation => _rotation;
    
    private float _movement;
    public float Movement => _movement;
    
    private bool _salute;
    public bool Salute => _salute;
    public UnityAction SaluteUp;
    public UnityAction SaluteDown;

    private void OnTurn(InputValue value)
    {
        _rotation = value.Get<float>();
    }
    private void OnWalk(InputValue value)
    {
        _movement = value.Get<float>();
    }
    private void OnSalute(InputValue value)
    {
        bool isPressed = value.isPressed;
        
        if (_salute != isPressed)
        {
            if (isPressed)
                SaluteDown?.Invoke();
            else
                SaluteUp?.Invoke();
        }
        
        _salute = isPressed;
        
    }
    
}
