using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerParent : MonoBehaviour
{
    public bool isTouchingWeakWall;
    public bool isTouchingIntro;
    public bool isTouchingLever;
    [HideInInspector]public GameObject weakWall;
    [HideInInspector]public IntroManager introMan;
    [HideInInspector]public Lever leverController;

    private void OnEnable()
    {
        isTouchingWeakWall = false;

    }
    private void OnDisable()
    {
        isTouchingWeakWall = false;
        isTouchingIntro = false;
        isTouchingLever = false;
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
        if (collision.CompareTag("Lever"))
        {
            leverController = collision.GetComponent<Lever>();
            isTouchingLever = true;
        }
    }
    
}
