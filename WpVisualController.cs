using UnityEngine;

public class WpVisualController : MonoBehaviour
{
    [SerializeField] Transform[] _wpTransforms;

    [SerializeField] Transform _pistol;
    [SerializeField] Transform _auto;
    [SerializeField] Transform _shotgun;
    [SerializeField] Transform _rifle;
    [SerializeField] Transform _revolver;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchOnGun(_pistol);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchOnGun(_auto);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SwitchOnGun(_shotgun);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SwitchOnGun(_rifle);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SwitchOnGun(_revolver);
        }
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
    }

}
