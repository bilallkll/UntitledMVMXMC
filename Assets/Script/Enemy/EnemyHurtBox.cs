using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyHurtBox : MonoBehaviour
{
    public bool isFly;
    public int health = 3;
    public Rigidbody2D rb;
    [Header("Death")]
    public Collider2D col;
    public Collider2D hurtCol;
    public Animator spriteAnim;
    public Instantiatedd instantiated;
    public GameObject deathEffect;
    [Header("Hurt Effect")]
    public GameObject hurtEffect;
    public float invicibilityTime = 0.4f;
    public Animator hurtAnim;
    public AudioSource hurtSfx;
    public float knockBack = 5;
    public float playerKnockBack = 4;
    PlayerController controller;
    public float pogoMultiplier = 1;
    public float knockBackTime = .2f;
    public SimpleEnemy enemy;
    public ObjectShake objShake;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerHitbox"))
        {
            TakeDamage(collision.ClosestPoint(transform.position));
        }
    }

    public void TakeDamage(Vector2 contactpos)
    {
        health--;
        if (hurtEffect) Instantiate(hurtEffect, contactpos, Quaternion.identity);

        if (health <= 0)
        {
            Death();
            return;
        }
        else
        {
            StartCoroutine(InvicibilityTime());
            DamageEffect();
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
        if(objShake)objShake.shake = true;
        if (hurtAnim) hurtAnim.SetTrigger("Hurt");
        if (hurtSfx) hurtSfx.Play();
    }

    public void Death()
    {
        if(rb)rb.isKinematic = true;
        if(spriteAnim)spriteAnim.SetTrigger("Death");
        if(col)col.enabled = false;
        instantiated.enabled = true;
        if(deathEffect)Instantiate(deathEffect, transform.position, Quaternion.identity);
    }

    public void KnockBack()
    {
        if (!controller._facingUp && !controller._facingDown)
        {
            controller.KnockBack(playerKnockBack);
        }
        if (isFly && (controller._facingUp || (controller._facingDown && !controller._grounded)))
        {
            if(rb)rb.velocity = new Vector2(rb.velocity.y, 0);
            if(rb)rb.AddForce((controller._facingUp ? Vector2.up : Vector2.down) * knockBack, ForceMode2D.Impulse);
            StartCoroutine(KnockBackTime());

        }
        else
        {
            if (rb) rb.velocity = new Vector2(rb.velocity.x, 0);

            if (rb) rb.AddForce((controller._facingRight ? Vector2.right : Vector2.left) * knockBack, ForceMode2D.Impulse);
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
        if(enemy == null) yield break;
        enemy.canMove = false;
        yield return new WaitForSeconds(knockBackTime);
        enemy.canMove = true;
    }
}
