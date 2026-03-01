using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerParent : MonoBehaviour
{
    public bool isTouchingWeakWall;
    public bool isTouchingIntro;
    [HideInInspector]public GameObject weakWall;
    [HideInInspector]public IntroManager introMan;

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
        if (collision.CompareTag("IntroTrigger"))
        {
            introMan = collision.GetComponent<IntroManager>();
            isTouchingIntro = true;
        }
    }
    
}
