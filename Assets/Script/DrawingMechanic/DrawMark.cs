using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawMark : MonoBehaviour
{
    public GlobalVariable globalVar;
    public Transform cursor; 
    public Drawingmech drawScript; 
    SpriteRenderer spriteRenderer;
    public bool isDrawn;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        float distance = Vector2.Distance(cursor.transform.position, transform.position);
        if (distance < globalVar.drawMarkTreshold && drawScript.isDrawing && !isDrawn)
        {
            spriteRenderer.color = Color.red;
            isDrawn = true;

            drawScript.MarkCheck(gameObject);
        }
        else if(drawScript.isDrawing == false && isDrawn)
        {
            ResetMark();
        }
    }
    public void ResetMark()
    {
        spriteRenderer.color = Color.white;
        isDrawn = false;

        drawScript.drawIndex = 0;
    }
}
