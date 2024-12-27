using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private float _fwdSpeed = 1.0f;
    [SerializeField] private float _rotationSpeed = 25.0f;
    [SerializeField] private float RotationSmoothTime;
    [SerializeField] private float _turnBackTime = 0.3f;
    [SerializeField] private Transform _moveRoot;
    [SerializeField] private Animator _animator;
    [SerializeField] private CharacterController _characterController;
    
    // Components
    private PlayerInputController _inputController;
    
    private GesturesManager _gesturesManager;
    private Camera _mainCamera;
    private float _targetRotation;
    float _rotationVelocity = 0f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _inputController = GetComponent<PlayerInputController>();
        //_characterController = GetComponent<CharacterController>();
        _gesturesManager = GetComponentInChildren<GesturesManager>();
        _mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (_inputController.Movement.magnitude > 0)
            _gesturesManager?.StopPlayGesture();
        else
            _gesturesManager?.StartPlayGesture();
        
        // normalise input direction
        Vector3 inputDirection = new Vector3(_inputController.Movement.x, 0.0f, _inputController.Movement.y).normalized;
        float inputMagnitude = inputDirection.magnitude;
        
        if (inputMagnitude > 0.01)
        {
            _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                              _mainCamera.transform.eulerAngles.y;
        }
        float rotation = Mathf.SmoothDampAngle(_moveRoot.eulerAngles.y,
            _targetRotation,
            ref _rotationVelocity,
            RotationSmoothTime);

        // rotate to face input direction relative to camera position
        _moveRoot.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);

        if (inputMagnitude > 0.01)
        {
            // move the player
            Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward * (_fwdSpeed * inputMagnitude);
            _characterController.SimpleMove(targetDirection);
        }
        _animator.SetFloat(AnimatorHandles.WalkSpeed, inputMagnitude);
    }
}
