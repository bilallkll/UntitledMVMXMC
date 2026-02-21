using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerParent : MonoBehaviour
{
    public bool isTouchingWeakWall;
    public GameObject weakWall;

    private void OnEnable()
    {
        isTouchingWeakWall = false;

    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("WeakWall"))
        {
            isTouchingWeakWall = true;
            weakWall = collision.gameObject;
        }
    }
    
}
