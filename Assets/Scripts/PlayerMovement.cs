using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    protected Rigidbody2D rb;

    [Header("Movement Debug")]
    public Vector2 movementInput;

    [Header("Movement Flags")]
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
    }

    private void Move()
    {
        Vector2 velocity = new Vector2(0.0f, 0.0f);
        velocity.x = movementInput.x * horizontalMoveSpeed;

        if(allowedVerticalInput)
        {
            velocity.y = movementInput.y * verticalMoveSpeed;
        }

        rb.velocity = velocity;
    }

    public void SetHorizontalInput(float x)
    {
        movementInput.x = x;
    }
    public void SetVerticalInput(float y)
    {
        movementInput.y = y;
    }
}
