using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    PlayerInputActions inputActions;
    PlayerMovement playerMovement;
    HealthComponent healthComponent; 
    private void Awake()
    {
        inputActions = new PlayerInputActions();
        playerMovement = GetComponent<PlayerMovement>();
        healthComponent = GetComponent<HealthComponent>();
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
        inputActions.Player.Horizontal.performed -= OnHorizontalCanceled;
        inputActions.Player.Vertical.performed -= OnVerticalCanceled;
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

    void OnHorizontalCanceled(InputAction.CallbackContext context)
    {
        playerMovement.SetHorizontalInput(0.0f);
    }

    void OnVerticalCanceled(InputAction.CallbackContext context)
    {
        playerMovement.SetVerticalInput(0.0f);
    }
}
