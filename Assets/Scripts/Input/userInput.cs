using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class userInput : MonoBehaviour
{
    public static PlayerInput PlayerInput;

    public static Vector2 moveInput;

    public static bool WasJumpPressed;
    public static bool IsJumpBeingPressed;
    public static bool WasJumpReleased;
    public static bool WasAttackPressed;
    public static bool WasInteractPressed;

    private InputAction _moveAction;
    private InputAction _jumpAction;
    private InputAction _attackAction;
    private InputAction _interactAction;

    private void Awake()
    {
        PlayerInput = GetComponent<PlayerInput>();

        _moveAction = PlayerInput.actions["Movement"];
        _jumpAction = PlayerInput.actions["Jump"];
        _attackAction = PlayerInput.actions["Attack"];
        _interactAction = PlayerInput.actions["Interact"];
    }

    private void Update()
    {
        // Movement
        moveInput = _moveAction.ReadValue<Vector2>();

        // Jump
        WasJumpPressed = _jumpAction.WasPerformedThisFrame();
        IsJumpBeingPressed = _jumpAction.IsPressed();
        WasJumpReleased = _jumpAction.WasReleasedThisFrame();

        // Attack
        WasAttackPressed = _attackAction.WasPressedThisFrame();

        // Interact
        WasInteractPressed = _interactAction.WasPressedThisFrame();

    }
    public static void DeactivatePlayerControls()
    {
        PlayerInput.currentActionMap.Disable();
    }
    public static void ActivatePlayerControls()
    {
        PlayerInput.currentActionMap.Enable(); // hello po welcome po sa jabee ano po order niyo?.. Isang ORDER PO NG PUTANG INANG BUHAY ITO... tulong :sob: :sob:
    }
}
