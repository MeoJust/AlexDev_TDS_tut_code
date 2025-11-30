using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    Player _player;
    Player_IA _controls;

    [Header("AimInfo")]
    [SerializeField] Transform _aimPoint;

    [Header("CameraInfo")]
    [SerializeField] Transform _camTarget;
    [SerializeField] float _minCameraDistance = 1.5f;
    [SerializeField] float _maxCameraDistance = 5f;
    [SerializeField] float _camSensitivity = 5f;
    [Space(10)]

    [SerializeField] LayerMask _aimLayerMask;

    Vector2 _aimInput;

    Vector3 _lookDir;
    RaycastHit _lastKnownMouseHit;

    void Start()
    {
        _player = GetComponent<Player>();
        AssignAimEvents();
    }

    void Update()
    {
        _aimPoint.position = GetMouseHitInfo().point;
        _aimPoint.position = new Vector3(_aimPoint.position.x, transform.position.y + 1.5f, _aimPoint.position.z);
        
        _camTarget.position = Vector3.Lerp(_camTarget.position, TargetCameraPosition(), _camSensitivity * Time.deltaTime);
    }

    Vector3 TargetCameraPosition()
    {
        float actualMaxCameraDistance = _player.Move.MoveInput.y < -.5f ? _minCameraDistance : _maxCameraDistance;

        Vector3 targetAimPosition = GetMouseHitInfo().point;
        Vector3 aimDir = (targetAimPosition - transform.position).normalized;

        float distanceToTargetPosition = Vector3.Distance(transform.position, targetAimPosition);

        float clampedDistance = Mathf.Clamp(distanceToTargetPosition, _minCameraDistance, actualMaxCameraDistance);

        targetAimPosition = transform.position + aimDir * clampedDistance;

        targetAimPosition.y = transform.position.y + 1.5f;

        return targetAimPosition;

    }
    public RaycastHit GetMouseHitInfo()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _aimLayerMask))
        {
            _lastKnownMouseHit = hit;
            return hit;
        }

        return _lastKnownMouseHit;
    }

    void AssignAimEvents()
    {
        _controls = _player.Controls;

        _controls.onFoot.aim.performed += ctx => _aimInput = ctx.ReadValue<Vector2>();
        _controls.onFoot.aim.canceled += ctx => _aimInput = Vector2.zero;
    }
}
