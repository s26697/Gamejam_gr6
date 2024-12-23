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
    [SerializeField] LayerMask bouncePadLayer;
    [SerializeField] LayerMask swingLayer;
    [SerializeField] LayerMask climbingLayer;

    [SerializeField] bool bouncePadActive;
    [SerializeField] bool ableToTrigger;
    [SerializeField] float lastButtonState;
    [SerializeField] float holdTime;
    [SerializeField] float jjumpForce = 25.0f;
    [SerializeField] float maxHoldTime;


    Animator anim;
    LineRenderer renderer;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
        playerMovement = GetComponent<PlayerMovement>();
        healthComponent = GetComponent<HealthComponent>();
        anim = GetComponent<Animator>();

        renderer = gameObject.AddComponent<LineRenderer>();

        renderer.startWidth = 0.15f;
        renderer.endWidth = 0.15f;
        renderer.material = new Material(Shader.Find("Sprites/Default"));
        renderer.startColor = new Color(24f / 255f, 50f / 255f, 92f / 255f);
        renderer.endColor = new Color(24f / 255f, 50f / 255f, 92f / 255f);
        renderer.positionCount = 0;
    }

    void Update()
    {
        if (bouncePadActive)
        {
            if (lastButtonState == -1)
            {
                holdTime += Time.deltaTime;

                if (holdTime > maxHoldTime)
                {
                    ableToTrigger = true;
                }
            }
        }

        if (playerMovement.isSwinging)
        {
            Vector2 swingLocation = playerMovement.swingLocation;
            Vector2 playerLocation = playerMovement.transform.position;

            renderer.positionCount = 2;
            renderer.SetPosition(0, new Vector3(playerLocation.x, playerLocation.y, 0));
            renderer.SetPosition(1, new Vector3(swingLocation.x, swingLocation.y, 0));
        }
        else
        {
            renderer.positionCount = 0;
        }

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
        inputActions.Player.Horizontal.performed -= OnHorizontal;
        inputActions.Player.Vertical.performed -= OnVertical;
        inputActions.Player.Swing.performed -= OnSwing;
    }

    void OnHorizontal(InputAction.CallbackContext context)
    {
        playerMovement.SetHorizontalInput(context.ReadValue<float>());
    }

    void OnVertical(InputAction.CallbackContext context)
    {
        float state = context.ReadValue<float>();
        if (playerMovement.allowedVerticalInput && state == 1)
        {
            anim.SetBool("isClimbing", true);
        }
        else
        {
            anim.SetBool("isClimbing", false);
        }
        if (state == -1) // Tutaj jednak xDDDDD
        {
            if (!playerMovement.allowedVerticalInput)
            {
                anim.SetBool("isCrouching", true);
            }
            else
            {
                anim.SetBool("isClimbing", true);
            }

            //transform.position -= new Vector3(0, 1, 0);
        }
        else
        {
            anim.SetBool("isCrouching", false);
        }
        playerMovement.SetVerticalInput(state);
        if (lastButtonState != state)
        {
            if (ableToTrigger)
            {
                playerMovement.TriggerJump(jjumpForce);
                bouncePadActive = false;
                ableToTrigger = false;
            }
            holdTime = 0.0f;
        }
        lastButtonState = state;
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
        else if (((1 << collision.gameObject.layer) & bouncePadLayer) != 0)
        {
            bouncePadActive = true;
            ableToTrigger = false;
        }
        else if (((1 << collision.gameObject.layer) & jumpPadLayer) != 0)
        {
            JumpPad jumpPad = collision.gameObject.GetComponent<JumpPad>();
            if (jumpPad != null)
            {
                playerMovement.TriggerJump(jumpPad.jumpForce);
            }
        }
        else if (((1 << collision.gameObject.layer) & climbingLayer) != 0)
        {
            playerMovement.allowedVerticalInput = true;

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & bouncePadLayer) != 0)
        {
            bouncePadActive = false;
            ableToTrigger = false;
        }
        if (((1 << collision.gameObject.layer) & climbingLayer) != 0)
        {
            playerMovement.allowedVerticalInput = false;

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
