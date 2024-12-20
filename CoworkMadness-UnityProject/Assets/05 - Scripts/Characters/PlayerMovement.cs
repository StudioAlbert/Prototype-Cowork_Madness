using UnityEngine;
using Quaternion = System.Numerics.Quaternion;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private float _fwdSpeed = 1.0f;
    [SerializeField] private float _rotationSpeed = 25.0f;
    [SerializeField] private float _turnBackTime = 0.3f;
    [SerializeField] private Transform _moveRoot;
    [SerializeField] private Animator _animator;

    // Components
    private PlayerInputController _inputController;
    private CharacterController _characterController;
    private GesturesManager _gesturesManager;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _inputController = GetComponent<PlayerInputController>();
        _characterController = GetComponent<CharacterController>();
        _gesturesManager = GetComponentInChildren<GesturesManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_inputController.Movement.magnitude > 0)

            _gesturesManager?.StopPlayGesture();
        else
            _gesturesManager?.StartPlayGesture();

        _moveRoot.LookAt(_moveRoot.position + new Vector3(_inputController.Movement.x, 0, _inputController.Movement.y));
        
        
        _animator.SetFloat(AnimatorHandles.WalkSpeed, _inputController.Movement.magnitude);

    }
}
