using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    PlayerInputActions inputActions;
    PlayerMovement playerMovement;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
        playerMovement = GetComponent<PlayerMovement>();
    }

    void OnEnable()
    {
        inputActions.Enable();
        inputActions.Player.Horizontal.performed += OnHorizontal;
        inputActions.Player.Vertical.performed += OnVertical;
    }

    void OnDisable()
    {
        inputActions.Disable();
        inputActions.Player.Horizontal.performed -= OnHorizontal;
        inputActions.Player.Vertical.performed -= OnVertical;
    }

    void Update()
    {
        
    }

    void OnHorizontal(InputAction.CallbackContext context)
    {
        playerMovement.SetHorizontalInput(context.ReadValue<float>());
    }
    void OnVertical(InputAction.CallbackContext context)
    {
        playerMovement.SetVerticalInput(context.ReadValue<float>());
    }
}
