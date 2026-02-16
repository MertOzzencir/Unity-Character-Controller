using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    private InputBase inputBase;


    void Awake()
    {
        if (Instance == null)
            Instance = this;
        inputBase = new InputBase();
        inputBase.Enable();
    }
    public Vector2 MovementVector()
    {
        return inputBase.Player.Move.ReadValue<Vector2>();
    }
    void OnEnable()
    {
        inputBase.Enable();
    }
    void Osable()
    {
        inputBase.Disable();
    }

}
