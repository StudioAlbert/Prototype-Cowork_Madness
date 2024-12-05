using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    
    [SerializeField] private float _fwdSpeed = 1.0f;
    [SerializeField] private float _rotationSpeed = 25.0f;
    
    // Components
    private PlayerInputController _inputController;
    private CharacterController _characterController;
    private GesturesManager _gesturesManager;
    [SerializeField] private Animator _animator;
    
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
        if(_inputController.Movement >= 0)
        {
            _gesturesManager?.StopPlayGesture();
            _characterController.SimpleMove(transform.forward * (_inputController.Movement * _fwdSpeed * Time.deltaTime));
            _characterController.transform.Rotate(Vector3.up, _inputController.Rotation * _rotationSpeed * Time.deltaTime);
        }
        else
        {
            _gesturesManager?.StartPlayGesture();
        }
        
        _animator.SetFloat(AnimatorHandles.WalkSpeed, _inputController.Movement);
            
    }
}
