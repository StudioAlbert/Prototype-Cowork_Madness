using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    
    [SerializeField] private float _fwdSpeed = 1.0f;
    [SerializeField] private float _rotationSpeed = 25.0f;
    
    private PlayerInputController _inputController;
    private CharacterController _characterController;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _inputController = GetComponent<PlayerInputController>();
        _characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(_inputController.Movement >= 0)
        {
            _characterController.SimpleMove(transform.forward * (_inputController.Movement * _fwdSpeed * Time.deltaTime));
            _characterController.transform.Rotate(Vector3.up, _inputController.Rotation * _rotationSpeed * Time.deltaTime);
        }
            
    }
}
