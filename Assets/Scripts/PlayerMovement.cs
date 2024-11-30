using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    protected Rigidbody2D rb;

    [Header("Movement Debug")]
    public Vector2 movementInput;
    public float jumpForce;
    public Vector2 swingLocation;

    [Header("Movement Flags")]
    [SerializeField] public bool performJump;
    [SerializeField] public bool allowedVerticalInput;
    [SerializeField] public bool isSwinging;

    [Header("Movement Settings")]
    [SerializeField] public float horizontalMoveSpeed;
    [SerializeField] public float verticalMoveSpeed;

    [Header("Swing Settings")]
    [SerializeField] public float swingGravity = 9.8f; // Gravitational force for swinging
    [SerializeField] public float angularDamping = 0.99f; // Damping factor for realistic motion
    [SerializeField] public float inputTorque = 2f; // Torque added by player input

    private float currentAngle; // Current angle relative to vertical
    private float angularVelocity; // Angular velocity for the pendulum
    private float swingLength; // Dynamic distance from player to anchor (pendulum length)

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (isSwinging)
        {
            HandleSwinging();
        }
        else
        {
            // Regular movement if not swinging
            HandleMovement();
        }
    }

    void FixedUpdate()
    {
        if (!isSwinging)
        {
            Move();
            if (performJump)
            {
                Jump();
            }
        }
    }

    private void HandleMovement()
    {
        // Update horizontal input for regular movement when not swinging
        if (!isSwinging)
        {
            Vector2 velocity = new Vector2(movementInput.x * horizontalMoveSpeed, rb.velocity.y);

            if (allowedVerticalInput)
            {
                velocity.y = movementInput.y * verticalMoveSpeed;
            }

            rb.velocity = velocity;
        }
    }

    private void Move()
    {
        // Regular movement here
        Vector2 velocity = new Vector2(movementInput.x * horizontalMoveSpeed, rb.velocity.y);

        if (allowedVerticalInput)
        {
            velocity.y = movementInput.y * verticalMoveSpeed;
        }

        rb.velocity = velocity;
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0f);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        performJump = false;
    }

    public void SetHorizontalInput(float x)
    {
        movementInput.x = x;
    }

    public void SetVerticalInput(float y)
    {
        movementInput.y = y;
    }

    public void TriggerJump(float force)
    {
        jumpForce = force;
        performJump = true;
    }

    public void SetSwingLocation(Vector2 location)
    {
        swingLocation = location;
        UpdateSwingLength();
        Vector2 toAnchor = (Vector2)transform.position - swingLocation;
        currentAngle = Mathf.Atan2(toAnchor.x, -toAnchor.y); // Calculate initial angle
    }

    public void StartSwing()
    {
        if (!isSwinging)
        {
            isSwinging = true;
            rb.bodyType = RigidbodyType2D.Kinematic; // Use kinematic body to control motion manually
            rb.velocity = Vector2.zero; // Reset velocity when starting to swing
        }
    }

    public void StopSwing()
    {
        if (isSwinging)
        {
            isSwinging = false;
            rb.bodyType = RigidbodyType2D.Dynamic; // Return to regular physics behavior
            rb.velocity = new Vector2(
                angularVelocity * swingLength * Mathf.Cos(currentAngle),
                angularVelocity * swingLength * Mathf.Sin(currentAngle)
            ); // Preserve momentum when exiting swing
        }
    }

    private void HandleSwinging()
    {
        // Recalculate the swing length based on the current distance from the anchor
        UpdateSwingLength();

        // Apply pendulum physics
        float gravityForce = swingGravity * Mathf.Sin(currentAngle); // Restoring force (pendulum)
        angularVelocity += -gravityForce / swingLength * Time.deltaTime; // Update angular velocity
        angularVelocity *= angularDamping; // Apply angular damping for smooth motion

        // Apply horizontal input as torque to influence swinging
        angularVelocity += movementInput.x * inputTorque * Time.deltaTime;

        // Update angle (change in angle due to velocity)
        currentAngle += angularVelocity * Time.deltaTime;

        // Calculate new position based on the angle
        Vector2 offset = new Vector2(
            Mathf.Sin(currentAngle),
            -Mathf.Cos(currentAngle)
        ) * swingLength;

        // Update player position to follow the swing
        rb.position = swingLocation + offset;
    }

    // This method updates the swing length dynamically based on the current distance to the anchor
    private void UpdateSwingLength()
    {
        swingLength = Vector2.Distance(transform.position, swingLocation);
    }
}
