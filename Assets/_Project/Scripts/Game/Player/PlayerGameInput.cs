using UnityEngine;

public class PlayerGameInput : MonoBehaviour
{
    private InputSystem_Actions inputActions;

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
        inputActions.Player.Enable();
    }

    private void OnDestroy()
    {
        if (inputActions != null)
            inputActions.Player.Disable();
    }

    /// <summary>
    /// Returns the movement input as a Vector2 (X = horizontal, Y = vertical).
    /// </summary>
    public Vector2 GetMoveValue()
    {
        return inputActions.Player.Move.ReadValue<Vector2>();
    }

    /// <summary>
    /// Returns the look input as a Vector2 (X = mouse delta X, Y = mouse delta Y).
    /// </summary>
    public Vector2 GetLookValue()
    {
        return inputActions.Player.Look.ReadValue<Vector2>();
    }

    /// <summary>
    /// Checks if the sprint button was pressed this frame.
    /// </summary>
    public bool IsSprintInputStart()
    {
        return inputActions.Player.Sprint.ReadValue<bool>();
    }

    /// <summary>
    /// Checks if the sprint button was released this frame.
    /// </summary>
    public bool IsSprintInputStop()
    {
        return inputActions.Player.Sprint.ReadValue<bool>();
    }

    public bool IsJumpInput()
    {
        return inputActions.Player.Jump.ReadValue<bool>();
    }
}
