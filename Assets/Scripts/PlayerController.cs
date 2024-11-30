using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    PlayerInputActions inputActions;
    PlayerMovement playerMovement;
<<<<<<< HEAD
    HealthComponent healthComponent;

    [SerializeField] LayerMask jumpPadLayer;
    [SerializeField] LayerMask swingLayer;

=======
    HealthComponent healthComponent; 
    GreaseComponent greaseComponent;
>>>>>>> 83e70511cd257f32c8cff27c20fdc5f1faf3efca
    private void Awake()
    {
        inputActions = new PlayerInputActions();
        playerMovement = GetComponent<PlayerMovement>();
        healthComponent = GetComponent<HealthComponent>();
        greaseComponent = GetComponent<GreaseComponent>();
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

<<<<<<< HEAD
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & jumpPadLayer) != 0)
        {
            JumpPad jumpPad = collision.gameObject.GetComponent<JumpPad>();
            if (jumpPad != null)
            {
                playerMovement.TriggerJump(jumpPad.jumpForce);
            }
        }
=======
    public void UseGrease(GreaseComponent grease)
    {
        healthComponent.DrinkGrease(grease); 
>>>>>>> 83e70511cd257f32c8cff27c20fdc5f1faf3efca
    }
}
