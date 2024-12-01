using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    PlayerInputActions inputActions;
    PlayerMovement playerMovement;
    HealthComponent healthComponent;
    [SerializeField] LayerMask jumpPadLayer;
    [SerializeField] LayerMask swingLayer;

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
        inputActions.Player.Swing.performed += OnSwing;
    }

    void OnDisable()
    {
        inputActions.Disable();
        inputActions.Player.Horizontal.performed -= OnHorizontalCanceled;
        inputActions.Player.Vertical.performed -= OnVerticalCanceled;
        inputActions.Player.Swing.performed -= OnSwing;
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

    void OnSwing(InputAction.CallbackContext context)
    {
        float distanceToSwingAnchor = Vector2.Distance(playerMovement.transform.position, playerMovement.swingLocation);

        if (playerMovement.isSwinging)
        {
            playerMovement.StopSwing();
        }
        else
        {
            if (distanceToSwingAnchor < playerMovement.maxSwingLength)
            {
                playerMovement.StartSwing();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & swingLayer) != 0)
        {
            CircleCollider2D swingCollider = collision.GetComponent<CircleCollider2D>();
            if (swingCollider != null)
            {
                Vector2 anchorLocation = collision.transform.position;
                float maxLength = swingCollider.radius * swingCollider.transform.lossyScale.x;

                playerMovement.SetSwingParameters(anchorLocation, maxLength);
            }
        }
        else if (((1 << collision.gameObject.layer) & jumpPadLayer) != 0)
        {
            JumpPad jumpPad = collision.gameObject.GetComponent<JumpPad>();
            if (jumpPad != null)
            {
                playerMovement.TriggerJump(jumpPad.jumpForce);
            }
        }
    }


    public void UseGrease(GreaseComponent grease)
    {
        healthComponent.DrinkGrease(grease);
    }

    public void UseScrap(ScrapComponent scrap)
    {
        healthComponent.Heal(scrap.HealValue * healthComponent.maxHealth / 100);
    }

    public void EatRobot(EatRobotComponent robot)
    {
        healthComponent.HealthAdd(robot);
        healthComponent.Heal(healthComponent.maxHealth);
        
    }
}

