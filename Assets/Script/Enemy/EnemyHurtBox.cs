using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyHurtBox : MonoBehaviour
{
    public bool isFly;
    public int health;
    public Rigidbody2D rb;
    [Header("Death")]
    public Collider2D col;
    public Collider2D hurtCol;
    public Animator spriteAnim;
    public Instantiatedd instantiated;
    [Header("Hurt Effect")]
    public float invicibilityTime;
    public Animator hurtAnim;
    public AudioSource hurtSfx;
    public float knockBack;
    public float playerKnockBack;
    PlayerController controller;
    public float pogoMultiplier = 1;
    public float knockBackTime;
    public SimpleEnemy enemy;

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
        StartCoroutine(InvicibilityTime());
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
            StartCoroutine(KnockBackTime());

        }
        else
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);

            rb?.AddForce((controller._facingRight ? Vector2.right : Vector2.left) * knockBack, ForceMode2D.Impulse);
            StartCoroutine(KnockBackTime());

        }


    }
    IEnumerator InvicibilityTime()
    {
        hurtCol.enabled = false;
        yield return new WaitForSeconds(invicibilityTime);
        hurtCol.enabled = true;
    }
    IEnumerator KnockBackTime()
    {
        enemy.canMove = false;
        yield return new WaitForSeconds(knockBackTime);
        enemy.canMove = true;
    }
}
