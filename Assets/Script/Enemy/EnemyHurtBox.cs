using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyHurtBox : MonoBehaviour
{
    public bool isFly;
    public int health = 3;
    public Rigidbody2D rb;
    public bool canMove = true;
    [Header("Death")]
    public Collider2D col;
    public Collider2D hurtCol;
    public Animator spriteAnim;
    public Instantiatedd instantiated;
    public GameObject deathEffect;
    public float timeStop = 0.05f;
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
    public ObjectShake objShake;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerHitbox"))
        {
            TakeDamage(collision.ClosestPoint(transform.position), 1);
        }
        if (collision.CompareTag("RespawnHitBox"))
        {
            TakeDamage(collision.ClosestPoint(transform.position), 1000);
        }
    }

    public void TakeDamage(Vector2 contactpos, int damage)
    {
        health -= damage;
        if (hurtEffect) Instantiate(hurtEffect, contactpos, Quaternion.identity);

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
        if(objShake)objShake.shake = true;
        if (hurtAnim) hurtAnim.SetTrigger("Hurt");
        if (hurtSfx) hurtSfx.Play();
    }

    public void Death()
    {
        StartCoroutine(stopTime(timeStop));
    }
    IEnumerator stopTime(float time)
    {
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(time);
        Time.timeScale = 1;
        if (rb) rb.isKinematic = true;
        if (col) col.enabled = false;
        instantiated.enabled = true;
        if (deathEffect) Instantiate(deathEffect, transform.position, Quaternion.identity);
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
        if (hurtCol == null) yield break;
        hurtCol.enabled = false;
        yield return new WaitForSeconds(invicibilityTime);
        hurtCol.enabled = true;
    }
    IEnumerator KnockBackTime()
    {
        canMove = false;

        yield return new WaitForSeconds(knockBackTime);
        canMove = true;
    }
}
