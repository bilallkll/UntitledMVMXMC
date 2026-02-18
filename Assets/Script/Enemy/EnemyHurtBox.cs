using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHurtBox : MonoBehaviour
{
    public bool isFly;
    public int health;
    public Rigidbody2D rb;
    [Header("Death")]
    public Collider2D col;
    public Animator spriteAnim;
    public Instantiatedd instantiated;
    [Header("Hurt Effect")]
    public Animator hurtAnim;
    public AudioSource hurtSfx;
    public float knockBack;
    public float playerKnockBack;
    PlayerController controller;
    public float pogoMultiplier = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerHitbox"))
        {
            TakeDamage();
        }
    }

    public void TakeDamage()
    {
        health--;
        DamageEffect();

        if (health <= 0)
        {
            Death();
            return;
        }



    }

    public void DamageEffect()
    {
        controller = GameObject.Find("Player").GetComponent<PlayerController>();
        if(controller._facingDown && controller._grounded == false)
        {
            controller.Pogo(pogoMultiplier); 
            if(isFly) KnockBack();
        }
        else
        {
            KnockBack();
        }
        hurtAnim?.SetTrigger("Hurt");
        hurtSfx?.Play();
    }

    public void Death()
    {
        rb.isKinematic = true;
        spriteAnim.SetTrigger("Death");
        col.enabled = false;
        instantiated.enabled = true;
    }

    public void KnockBack()
    {
        if (!controller._facingUp && !controller._facingDown)
        {
            controller.KnockBack(playerKnockBack);
        }
        if (isFly && (controller._facingUp || (controller._facingDown && !controller._grounded)))
        {
            rb.velocity = new Vector2(rb.velocity.y, 0);
            rb?.AddForce((controller._facingUp ? Vector2.up : Vector2.down) * knockBack, ForceMode2D.Impulse);
        }
        else
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);

            rb?.AddForce((controller._facingRight ? Vector2.right : Vector2.left) * knockBack, ForceMode2D.Impulse);
        }


    }
}
