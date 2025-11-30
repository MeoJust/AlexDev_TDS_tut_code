using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerWpVisual : MonoBehaviour
{
    Animator _animator;
    Rig _rig;

    [SerializeField] Transform[] _wpTransforms;

    [SerializeField] Transform _pistol;
    [SerializeField] Transform _auto;
    [SerializeField] Transform _shotgun;
    [SerializeField] Transform _rifle;
    [SerializeField] Transform _revolver;

    [Header("Rig")]
    [SerializeField] float _rigIncreaceStep = .15f;

    [Header("Left Hand IK")]
    [SerializeField] TwoBoneIKConstraint _leftHandIK;
    [SerializeField] Transform _leftHandTransform;
    [SerializeField] float _leftHandIKIncreaceStep = .15f;

    Transform _currentGunTransform;
    bool _shouldIncreaseRig = false;
    bool _shouldIncreaseLeftHandIK = false;
    bool _isGrabbingWp = false;

    void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        _rig = GetComponentInChildren<Rig>();
        //TODO: Not in use yet
        // SwitchOnGun(_pistol);
    }

    void Update()
    {
        CheckWpSwitch();

        if (Input.GetKeyDown(KeyCode.R) && !_isGrabbingWp)
        {
            _animator.SetTrigger("Reload");
            _rig.weight = 0;
        }

        UpdateRigWeight();
        UpdateLeftHandIKWeight();
    }

    void UpdateRigWeight()
    {
        if (_shouldIncreaseRig)
        {
            _rig.weight += _rigIncreaceStep * Time.deltaTime;
            if (_rig.weight >= 1)
            {
                _shouldIncreaseRig = false;
            }
        }
    }

    void UpdateLeftHandIKWeight()
    {
        if (_shouldIncreaseLeftHandIK)
        {
            _leftHandIK.weight += _leftHandIKIncreaceStep * Time.deltaTime;
            if (_leftHandIK.weight >= 1)
            {
                _shouldIncreaseLeftHandIK = false;
            }
        }
    }

    void PlayWpGrabAnim(GrabType grabType)
    {
        _animator.SetInteger("GrabType", (int)grabType);
        _isGrabbingWp = true;
        
        if(_leftHandIK)
            _leftHandIK.weight = 0;

        _animator.SetTrigger("Grab");
        SetBusyGrabbingWp(true);
    }

    public void SetBusyGrabbingWp(bool isBusy)
    {
        _isGrabbingWp = isBusy;
        _animator.SetBool("isGrabbingWp", isBusy);
    }

    void SwitchOffGuns()
    {
        for (int i = 0; i < _wpTransforms.Length; i++)
        {
            _wpTransforms[i].gameObject.SetActive(false);
        }
    }

    void SwitchOnGun(Transform wpTransform)
    {
        SwitchOffGuns();
        wpTransform.gameObject.SetActive(true);
        _currentGunTransform = wpTransform;
        //TODO: Not in use yet
        // AttachLeftHand();
    }

    void AttachLeftHand()
    {
        Transform targetTransform = _currentGunTransform.GetComponentInChildren<LeftHandTargetTransform>().transform;

        _leftHandTransform.localPosition = targetTransform.localPosition;
        _leftHandTransform.localRotation = targetTransform.localRotation;
    }

    void SwitchAnimLayer(int layerIndex)
    {
        for (int i = 0; i < _animator.layerCount; i++)
        {
            if (i == layerIndex)
            {
                _animator.SetLayerWeight(i, 0);
            }

            _animator.SetLayerWeight(i, 1);

        }
    }

    public void ReturnRigWeightToOne() => _shouldIncreaseRig = true;
    public void ReturnLeftHandIKWeightToOne() => _shouldIncreaseLeftHandIK = true;

    void CheckWpSwitch()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchOnGun(_pistol);
            SwitchAnimLayer(0);
            PlayWpGrabAnim(GrabType.SideGrab);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchOnGun(_auto);
            SwitchAnimLayer(1);
            PlayWpGrabAnim(GrabType.BackGrab);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SwitchOnGun(_shotgun);
            SwitchAnimLayer(2);
            PlayWpGrabAnim(GrabType.BackGrab);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SwitchOnGun(_rifle);
            SwitchAnimLayer(3);
            PlayWpGrabAnim(GrabType.BackGrab);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SwitchOnGun(_revolver);
            SwitchAnimLayer(4);
            PlayWpGrabAnim(GrabType.SideGrab);
        }
    }
}

public enum GrabType { SideGrab, BackGrab };
