using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class userInput : MonoBehaviour
{
    public static PlayerInput PlayerInput;

    public static Vector2 moveInput;
    public static Vector2 pointerInput;

    public static bool WasJumpPressed;
    public static bool IsJumpBeingPressed;
    public static bool WasJumpReleased;
    public static bool WasAttackPressed;
    public static bool IsAttackBeingPressed;
    public static bool WasInteractPressed;

    private InputAction _moveAction;
    private InputAction _jumpAction;
    private InputAction _attackAction;
    private InputAction _interactAction;

    private InputAction _pointerAction;

    private void Awake()
    {
        PlayerInput = GetComponent<PlayerInput>();

        _moveAction = PlayerInput.actions["Movement"];
        _attackAction = PlayerInput.actions["Attack"];
        _interactAction = PlayerInput.actions["Interact"];
        _jumpAction = PlayerInput.actions["Jump"];
        _pointerAction = PlayerInput.actions["PointerPosition"];
        // Enable the action map if it's not already enabled
        if (!PlayerInput.currentActionMap.enabled)
        {
            PlayerInput.currentActionMap.Enable();
        }

        // Add debug logging to verify input detection
        //_attackAction.performed += ctx => Debug.Log("Attack performed");
        //_attackAction.started += ctx => Debug.Log("Attack started");
        //_attackAction.canceled += ctx => Debug.Log("Attack canceled");
    }

    private void Update()
    {
        // Movement
        moveInput = _moveAction.ReadValue<Vector2>();

        // Pointer
        pointerInput = _pointerAction.ReadValue<Vector2>();

        // Jump
        WasJumpPressed = _jumpAction.WasPerformedThisFrame();
        IsJumpBeingPressed = _jumpAction.IsPressed();
        WasJumpReleased = _jumpAction.WasReleasedThisFrame();

        // Attack
        WasAttackPressed = _attackAction.WasPressedThisFrame();
        IsAttackBeingPressed = _attackAction.IsPressed();

        // Interact
        WasInteractPressed = _interactAction.WasPressedThisFrame();
    }
    public static void DeactivatePlayerControls()
    {
        PlayerInput.currentActionMap.Disable();
    }
    public static void ActivatePlayerControls()
    {
        PlayerInput.currentActionMap.Enable();
    }

    // Add this to check if actions are properly set up
    private void OnEnable()
    {
        //Debug.Log($"Attack action path: {_attackAction?.actionMap?.name}/{_attackAction?.name}");
        //Debug.Log($"Attack action enabled: {_attackAction?.enabled}");
    }
}
