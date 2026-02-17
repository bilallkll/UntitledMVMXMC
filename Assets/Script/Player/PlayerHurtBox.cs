using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHurtBox : MonoBehaviour
{
    public int health;
    public PlayerController controller;
    public Rigidbody2D rb;
    public Vector2 deathForce;
    public float hurTime;
    public float deathDrag;
    Vector3 contactPoint;
    public CapsuleCollider2D col;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyHitBox"))
        {
            TakeDamage();
            contactPoint = collision.transform.position;
        }
    }
    public void TakeDamage()
    {
        health--;
        DamageEffect();

        if (health <= 0)
        {
            return;
        }



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
}
