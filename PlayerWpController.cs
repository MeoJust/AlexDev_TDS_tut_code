using UnityEngine;

public class PlayerWpController : MonoBehaviour
{
    Player _player;

    void Start()
    {
        _player = GetComponent<Player>();

        _player.Controls.onFoot.attack.performed += ctx => Attack();
    }

    void Attack()
    {
        //TODO: Not in use yet
        GetComponentInChildren<Animator>().SetTrigger("Attack");
    }
}
