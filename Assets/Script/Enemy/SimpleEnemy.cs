using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEnemy : MonoBehaviour
{
    [Header("Component")]
    Rigidbody2D rb;
    [Header("Ground Detection")]
    public Vector2 groundRaycastPos;
    [Header("Bools")]
    bool facingRight;
    public bool canMove = true;
    bool ground;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        Move();
    }
    public void Move()
    {
        if (!canMove) return;
        rb.velocity = new Vector2(facingRight ? 1 : -1, rb.velocity.y);
    }
    public void GroundCheck()
    {
        //ground = Physics2D.Raycast(groundRaycastPos, Vector2.down, 0.05f, groundMask);
    }
}
