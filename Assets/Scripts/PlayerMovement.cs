using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    protected Rigidbody2D rb;

    [Header("Movement Debug")]
    public Vector2 movementInput;
    public float jumpForce;

    [Header("Movement Flags")]
    [SerializeField] public bool performJump;
    [SerializeField] public bool allowedVerticalInput;

    [Header("Movement Settings")]
    [SerializeField] float horizontalMoveSpeed;
    [SerializeField] float verticalMoveSpeed;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {

    }

    void FixedUpdate()
    {
        Move();

        if (performJump)
        {
            Jump();
        }
    }

    private void Move()
    {
        Vector2 velocity = new Vector2(0.0f, rb.velocity.y); // Preserve vertical velocity
        velocity.x = movementInput.x * horizontalMoveSpeed;

        if (allowedVerticalInput)
        {
            velocity.y = movementInput.y * verticalMoveSpeed;
        }

        rb.velocity = velocity;
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0f); // Reset vertical velocity for consistent jump
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        performJump = false; // Reset the jump flag
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
}
