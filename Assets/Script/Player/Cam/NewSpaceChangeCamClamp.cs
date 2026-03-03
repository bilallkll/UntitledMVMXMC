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
    private void OnDrawGizmosSelected()
    {
        Camera cam = Camera.main;
        if (cam == null) return;

        float camHeight = cam.orthographicSize;
        float camWidth = camHeight * cam.aspect;

        float minX = newHorizontalClamp.x - camWidth;
        float maxX = newHorizontalClamp.y + camWidth;
        float minY = newVerticalClamp.x - camHeight;
        float maxY = newVerticalClamp.y + camHeight;

        Vector2 center = new Vector2(
            (minX + maxX) / 2f,
            (minY + maxY) / 2f
        );

        Vector2 size = new Vector2(
            maxX - minX,
            maxY - minY
        );

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(center, size);
    }
}
