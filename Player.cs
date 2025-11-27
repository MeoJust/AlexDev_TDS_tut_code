using UnityEngine;

public class Player : MonoBehaviour
{
    public Player_IA Controls;

    void Awake()
    {
        Controls = new Player_IA();
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
