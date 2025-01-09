using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float _panTime = 0.25f;
    
    private CinemachinePanTilt _panTilt;
    private float _panAxisAngle;
    private float _smoothVelocity;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _panTilt = GetComponent<CinemachinePanTilt>();
        if (_panTilt != null)
        {
            _panAxisAngle = _panTilt.PanAxis.Value;
        }
    }

    // Update is called once per frame
    void Update()
    {
        _panTilt.PanAxis.Value = Mathf.SmoothDamp(_panTilt.PanAxis.Value, _panAxisAngle, ref _smoothVelocity, _panTime);
    }

    void OnPivotRight(InputValue value)
    {
        if (!value.isPressed)
        {
            _panAxisAngle -= 90f;
        }
    }

    void OnPivotLeft(InputValue value)
    {
        if (!value.isPressed)
        {
            _panAxisAngle += 90f;
        }
    }

}
