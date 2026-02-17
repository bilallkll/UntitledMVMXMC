using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorridorsCamera : MonoBehaviour
{
    CamFollow cam;
    public Vector2 thisHorizontalClamp;
    public Vector2 thisVerticalClamp;
    Vector2 originalHorizontalClamp;
    Vector2 originalVerticalClamp;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            cam = GameObject.Find("Main Camera").GetComponent<CamFollow>();

            originalHorizontalClamp = cam.maxHorizontal;
            originalVerticalClamp = cam.maxVertical;

            cam.maxHorizontal = thisHorizontalClamp;
            cam.maxVertical = thisVerticalClamp;

        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && cam.newSpace == false)
        {
            cam.maxHorizontal = originalHorizontalClamp;
            cam.maxVertical = originalVerticalClamp;
        }
    }
}
