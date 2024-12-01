using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    protected Rigidbody2D rb;

    public Vector2 movementInput;
    public float jumpForce;

    [SerializeField] public bool performJump;
    [SerializeField] public bool allowedVerticalInput;
    [SerializeField] public bool isSwinging;

    [SerializeField] public float horizontalMoveSpeed;
    [SerializeField] public float verticalMoveSpeed;

    [SerializeField] public float swingGravity;
    [SerializeField] public float angularDamping;
    [SerializeField] public float inputTorque;

    public Vector2 swingLocation;
    public float maxSwingLength;
    protected float currentAngle;
    protected float angularVelocity;

    [SerializeField] public float impulseStrength = 10f;
    private Vector2 releaseVelocity;
    private bool isImpulseApplied = false;

    // New variables for smooth impulse
    private float impulseTime = 0f; // Time during which impulse is applied
    private float maxImpulseTime = 0.5f; // Duration of impulse application (seconds)
    private Vector2 impulseDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (isSwinging)
        {
            HandleSwinging();
        }
        else
        {
            Move();
            if (performJump)
            {
                Jump();
            }

            // Apply gradual impulse after swinging stops
            if (isImpulseApplied)
            {
                ApplyGradualImpulseOnStop();
                isImpulseApplied = false;
            }
        }
    }

    private void Move()
    {
        // Calculate the desired velocity based on input
        Vector2 targetVelocity = new Vector2(movementInput.x * horizontalMoveSpeed, rb.velocity.y);

        // Smoothly change the current velocity towards the target velocity
        Vector2 velocityDifference = targetVelocity - rb.velocity;

        // Apply a force in the direction of the velocity difference
        rb.AddForce(velocityDifference * 10f, ForceMode2D.Force); // You can adjust the multiplier for smoothness

        // Apply vertical input if allowed
        if (allowedVerticalInput)
        {
            Vector2 verticalForce = new Vector2(0f, movementInput.y * verticalMoveSpeed);
            rb.AddForce(verticalForce, ForceMode2D.Force);
        }
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
    public void SetSwingParameters(Vector2 location, float maxLength)
    {
        swingLocation = location;
        maxSwingLength = maxLength;
    }

    public void StartSwing()
    {
        isSwinging = true;
    }

    public void StopSwing()
    {
        // Mark the impulse as ready to be applied gradually
        isImpulseApplied = true;
        impulseTime = 0f; // Reset the impulse time
        isSwinging = false;
    }

    private void HandleSwinging()
    {
        Vector2 toAnchor = swingLocation - rb.position;
        float swingLength = toAnchor.magnitude;

        // Ensure that the rope length stays constant
        if (swingLength > maxSwingLength)
        {
            toAnchor = toAnchor.normalized * maxSwingLength;  // Correct the player's position to stay on the rope length
            rb.position = swingLocation - toAnchor;
            swingLength = maxSwingLength;
        }

        float angleFromY = Mathf.Atan2(toAnchor.x, -toAnchor.y);
        float tangentialForce = -swingGravity * Mathf.Sin(angleFromY);

        angularVelocity += (tangentialForce / swingLength) * Time.fixedDeltaTime;
        angularVelocity *= (1 - angularDamping * Time.fixedDeltaTime);

        float inputTorqueAdjusted = -movementInput.x * inputTorque * Time.fixedDeltaTime;
        if (Mathf.Abs(angularVelocity) < 10f)
        {
            angularVelocity += inputTorqueAdjusted;
        }

        Vector2 tangentialDirection = new Vector2(-toAnchor.y, toAnchor.x).normalized;
        Vector2 tangentialVelocity = tangentialDirection * angularVelocity * swingLength;

        rb.velocity = tangentialVelocity;

        Vector2 tensionForce = toAnchor.normalized * (swingGravity * Mathf.Cos(angleFromY) - Vector2.Dot(rb.velocity, toAnchor) / swingLength);
        rb.AddForce(tensionForce);
    }

    private void ApplyGradualImpulseOnStop()
    {
        // When the player stops swinging, capture the current velocity.
        Vector2 currentVelocity = rb.velocity;

        // Calculate the direction of the momentum (both horizontal and vertical).
        Vector2 releaseDirection = currentVelocity.normalized;  // Normalize the direction of release

        // Calculate the tangential impulse based on the player's velocity at release
        Vector2 tangentialImpulse = releaseDirection * impulseStrength;

        // Add upward component proportional to the vertical velocity at release
        float upwardImpulse = Mathf.Abs(currentVelocity.y) * 0.5f;
        tangentialImpulse.y += upwardImpulse;  // Add some upward boost for realism

        // Gradually apply the impulse based on time (simulating energy transfer)
        if (impulseTime < maxImpulseTime)
        {
            impulseTime += Time.fixedDeltaTime;  // Increase time with each FixedUpdate

            // Smoothly interpolate between 0 and the full impulse strength
            float lerpFactor = impulseTime / maxImpulseTime;

            // Apply the gradual impulse
            rb.AddForce(tangentialImpulse * lerpFactor, ForceMode2D.Force);
        }
    }
}
