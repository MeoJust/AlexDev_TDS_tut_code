using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerMove : MonoBehaviour
{
    Player_IA _controls;
    Player _player;
    CharacterController _controller;
    Animator _animator;

    [Header("MovementInfo")]
    [SerializeField] float _walkSpeed = 4f;
    [SerializeField] float _runSpeed = 7f;
    [SerializeField] float _turnSpeed = 10f;

    bool _isRunning = false;
    float _verticalVelocity;
    float _moveSpeed;

    Vector3 _moveDir;

    public Vector2 MoveInput { get; private set; }

    void Start()
    {
        _player = GetComponent<Player>();
        _controller = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();

        _moveSpeed = _walkSpeed;

        AssignMovementEvents();
    }

    void Update()
    {
        ApplyMovement();
        ApplyRotation();
        //AnimatorController();
    }

    //TODO: Not in use yet
    void AnimatorController()
    {
        float xVelocity = Vector3.Dot(_moveDir.normalized, transform.right);
        float zVelocity = Vector3.Dot(_moveDir.normalized, transform.forward);

        _animator.SetFloat("xVelocity", xVelocity, .1f, Time.deltaTime);
        _animator.SetFloat("zVelocity", zVelocity, .1f, Time.deltaTime);

        bool isRunning = _isRunning && _moveDir.magnitude > 0;
        _animator.SetBool("isRunning", isRunning);
    }

    void ApplyMovement()
    {
        _moveDir = new Vector3(MoveInput.x, 0, MoveInput.y);
        ApplyGravity();

        if (_moveDir.magnitude > 0)
        {
            _controller.Move(_moveDir * _moveSpeed * Time.deltaTime);
        }
    }

    void ApplyRotation()
    {
        Vector3 lookDir = _player.Aim.GetMouseHitInfo().point - transform.position;
        lookDir.y = 0;
        lookDir.Normalize();

        Quaternion targetRotation = Quaternion.LookRotation(lookDir);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _turnSpeed * Time.deltaTime);

    }

    void ApplyGravity()
    {
        if (!_controller.isGrounded)
        {
            _verticalVelocity -= 9.81f * Time.deltaTime;
            _moveDir.y = _verticalVelocity;
        }
        else
        {
            _verticalVelocity = -.5f;
        }
    }

    void AssignMovementEvents()
    {
        _controls = _player.Controls;

        _controls.onFoot.move.performed += ctx => MoveInput = ctx.ReadValue<Vector2>();
        _controls.onFoot.move.canceled += ctx => MoveInput = Vector2.zero;

        _controls.onFoot.run.performed += ctx =>
        {
            _isRunning = true;
            _moveSpeed = _runSpeed;

        };
        _controls.onFoot.run.canceled += ctx =>
        {
            _isRunning = false;
            _moveSpeed = _walkSpeed;
        };
    }
}
