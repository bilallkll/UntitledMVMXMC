using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedCamSpaces : MonoBehaviour
{
    public Transform thisLockPos;
    CamFollow cam;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            cam = GameObject.Find("Main Camera").GetComponent<CamFollow>();
            cam.followingPlayer = false;
            cam.target = thisLockPos;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            cam.followingPlayer = true;
            cam.target = GameObject.Find("Player").transform;
        }
    }
}
