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

    [Header("AimInfo")]
    [SerializeField] LayerMask _aimLayerMask;
    [SerializeField] Transform _aimPoint;

    Vector3 _lookDir;
    bool _isRunning = false;
    float _verticalVelocity;
    float _moveSpeed;

    Vector3 _moveDir;

    Vector2 _moveInput;
    Vector2 _aimInput;

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
        _moveDir = new Vector3(_moveInput.x, 0, _moveInput.y);
        ApplyGravity();

        if (_moveDir.magnitude > 0)
        {
            _controller.Move(_moveDir * _moveSpeed * Time.deltaTime);
        }
    }

    void ApplyRotation()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _aimLayerMask))
        {
            _lookDir = hit.point - transform.position;
            _lookDir.y = 0;
            _lookDir.Normalize();

            transform.forward = _lookDir;

            _aimPoint.position = new Vector3(hit.point.x, transform.position.y + 1.5f, hit.point.z);
        }

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

        _controls.onFoot.move.performed += ctx => _moveInput = ctx.ReadValue<Vector2>();
        _controls.onFoot.move.canceled += ctx => _moveInput = Vector2.zero;

        _controls.onFoot.aim.performed += ctx => _aimInput = ctx.ReadValue<Vector2>();
        _controls.onFoot.aim.canceled += ctx => _aimInput = Vector2.zero;

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
