using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEnemy : MonoBehaviour
{
    [Header("Value")]
    public float speed = 1;
    public float groundDetectHeight = 1;

    [Header("Component")]
    Rigidbody2D rb;

    [Header("Wall Detection")]
    public float wallDetectHeight;
    [Header("Ground Detection")]
    public Vector2 rightGroundRaycastPos;
    public Vector2 leftGroundRaycastPos;
    public GlobalVariable globalVar;
    [Header("Bools")]
    public bool facingRight;
    public bool canMove = true;
    bool groundLeft;
    bool groundRight;
    bool wallLeft;
    bool wallRight;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        GroundCheck();
        WallCheck();
    }
    private void Update()
    {
        Move();
        Direction();
    }
    public void Direction()
    {
        if (!groundLeft && !groundRight) return;

        if(!groundLeft && !facingRight)
        {
            facingRight = true;
        }
        else if(!groundRight && facingRight)
        {
            facingRight = false;

        }
        else if(wallLeft && !facingRight)
        {
            facingRight = true;
        }
        else if (wallRight && facingRight)
        {
            facingRight = false;

        }
    }
    public void Move()
    {
        if (!canMove) return;
        rb.velocity = new Vector2((facingRight ? 1 : -1) * speed, rb.velocity.y);
    }
    public void GroundCheck()
    {
        RaycastHit2D hitLeft = Physics2D.Raycast((Vector2)transform.position + leftGroundRaycastPos, Vector2.down, groundDetectHeight, globalVar.groundMask);
        RaycastHit2D hitRight = Physics2D.Raycast((Vector2)transform.position + rightGroundRaycastPos, Vector2.down, groundDetectHeight, globalVar.groundMask);

        groundLeft = hitLeft.collider != null;
        groundRight = hitRight.collider != null;


        Debug.DrawRay((Vector2)transform.position + rightGroundRaycastPos, Vector2.down * groundDetectHeight, Color.magenta);
        Debug.DrawRay((Vector2)transform.position + leftGroundRaycastPos, Vector2.down * groundDetectHeight, Color.magenta);
    }
    public void WallCheck()
    {
        RaycastHit2D hitLeft = Physics2D.Raycast((Vector2)transform.position + Vector2.down * .5f, Vector2.left, wallDetectHeight, globalVar.groundMask);
        RaycastHit2D hitRight = Physics2D.Raycast((Vector2)transform.position + Vector2.down * .5f, Vector2.right, wallDetectHeight, globalVar.groundMask);

        wallLeft = hitLeft.collider != null;
        wallRight = hitRight.collider != null;


        Debug.DrawRay((Vector2)transform.position + Vector2.down * .5f, Vector2.left * wallDetectHeight, Color.magenta);
        Debug.DrawRay((Vector2)transform.position + Vector2.down * .5f, Vector2.right * wallDetectHeight, Color.magenta);
    }
}
             