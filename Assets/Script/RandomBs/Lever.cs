using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    Animator leverAnim;
    public Animator doorAnim;
    private void Start()
    {
        leverAnim = GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerHitbox"))
        {
            ActivateLever();
        }
    }
    public void ActivateLever()
    {
        leverAnim.SetTrigger("Activate");
        doorAnim.SetTrigger("Activate");
    }

}
