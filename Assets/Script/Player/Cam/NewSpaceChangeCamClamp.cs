using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewSpaceChangeCamClamp : MonoBehaviour
{
    CamFollow cam;
    public Vector2 newHorizontalClamp;
    public Vector2 newVerticalClamp;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            cam = GameObject.Find("Main Camera").GetComponent<CamFollow>();

            cam.maxHorizontal = newHorizontalClamp;
            cam.maxVertical = newVerticalClamp;
            cam.newSpace = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            cam.newSpace = false;
        }

    }
}
