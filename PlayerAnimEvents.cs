using UnityEngine;

public class PlayerAnimEvents : MonoBehaviour
{
    PlayerWpVisual _playerWpVisual;

    void Start()
    {
        _playerWpVisual = GetComponentInParent<PlayerWpVisual>();
    }

    public void ReloadIsDone()
    {
        _playerWpVisual.ReturnRigWeightToOne();
    }


    //TODO: Should be called from AnimationEvent
    public void ReturnRig()
    {
        _playerWpVisual.ReturnRigWeightToOne();
        _playerWpVisual.ReturnLeftHandIKWeightToOne();
    }

    //TODO: Should be called from AnimationEvent
    public void GrabIsDone()
    {

        _playerWpVisual.SetBusyGrabbingWp(false);
    }
}
