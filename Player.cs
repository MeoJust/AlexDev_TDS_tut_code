using UnityEngine;

public class Player : MonoBehaviour
{
    public Player_IA Controls { get; private set; } // { get; private set; } means that the property is read-only
    public PlayerAim Aim { get; private set; }
    public PlayerMove Move { get; private set; }

    void Awake()
    {
        Controls = new Player_IA();
        Aim = GetComponent<PlayerAim>();
        Move = GetComponent<PlayerMove>();
    }

    void OnEnable()
    {
        Controls.Enable();
    }

    void OnDisable()
    {
        Controls.Disable();
    }
}
