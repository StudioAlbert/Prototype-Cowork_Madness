using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    private float _rotation;
    public float Rotation => _rotation;
    
    private float _movement;
    public float Movement => _movement;
    
    private bool _salute;
    public bool Salute => _salute;

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
        _salute = value.isPressed;
    }
    
}
