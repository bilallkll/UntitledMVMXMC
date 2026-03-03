using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHurtBox : MonoBehaviour
{
    public int health;
    public GameObject[] healthIcons;
    public PlayerController controller;
    public Rigidbody2D rb;
    public Vector2 deathForce;
    public float hurTime;
    public float deathDrag;
    Vector3 contactPoint;
    public CapsuleCollider2D col;
    public float lastGroundedPosDistance;
    public Vector2 lastGroundedPos;
    bool cliffLeft;
    bool cliffRight;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyHitBox"))
        {
            TakeDamage();
            contactPoint = collision.transform.position;
        }
        if(collision.CompareTag("RespawnHitBox"))
        {
            TakeDamage(true);
            contactPoint = collision.transform.position;
        }
    }
    public void TakeDamage(bool respawn = false)
    {
        health--;
        healthIcons[health].SetActive(false);
        if (respawn)
        {
            RespawnEffect();
        }
        else
        {
            DamageEffect();
        }

        if (health <= 0)
        {
            return;
        }



    }
    private void FixedUpdate()
    {
        LastGrounded();
    }
    public void RespawnEffect()
    {
        controller._dash = false;
        controller._hurt = true;
        controller.disableMovement = true;
        rb.velocity = Vector2.zero;
        rb.drag = deathDrag;
        controller.rb.transform.position = lastGroundedPos;
        StartCoroutine(HurtTime());
    }
    public void DamageEffect()
    {
        controller._dash = false;
        rb.gravityScale = 1;
        controller._hurt = true;
        col.direction = CapsuleDirection2D.Horizontal;
        controller.disableMovement = true;
        rb.velocity = Vector2.zero;
        rb.drag = deathDrag;
        rb.AddForce(Vector2.up * deathForce.y + (transform.position.x > contactPoint.x ? Vector2.right : Vector2.left) * deathForce.x, ForceMode2D.Impulse);
        StartCoroutine(HurtTime());
    }
    IEnumerator HurtTime()
    {
        yield return new WaitForSeconds(hurTime);
        controller.disableMovement = false; 
        col.direction = CapsuleDirection2D.Vertical;
        controller._hurt = false;
    }

    public void LastGrounded()
    {
        if (controller._grounded && !controller.disableMovement)
        {
            float dist = Vector2.Distance(transform.position ,lastGroundedPos);
            if (dist >= lastGroundedPosDistance)
            {
                lastGroundedPos = controller.transform.position;
            }
        }
    }
}
