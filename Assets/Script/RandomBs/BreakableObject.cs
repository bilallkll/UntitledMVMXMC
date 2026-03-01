using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObject : MonoBehaviour
{
    [Header("Value")]
    public int health;
    [Header("BreakEffect")]
    public GameObject breakEffect;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerHitbox"))
        {
            health--;
        }
    }
}
