using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayer: MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Get input from keyboard
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement = movement.normalized; // Prevent faster diagonal movement
    }

    void FixedUpdate()
    {
        // Move the character
        rb.velocity = movement * moveSpeed;
    }
}
