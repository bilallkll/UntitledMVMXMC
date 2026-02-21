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
    public bool canBeNext;
    public int solutionNumber;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    void Update()
    {
        float distance = Vector2.Distance(cursor.transform.position, transform.position);
        if (distance < globalVar.drawMarkTreshold[solutionNumber] && drawScript.isDrawing && !isDrawn && canBeNext)
        {
            spriteRenderer.color = Color.red;
            isDrawn = true;

            drawScript.MarkCheck(gameObject);
        }
        else if(drawScript.isDrawing == false && isDrawn)
        {
            ResetMark();
        }
        if (!canBeNext)
        {
            spriteRenderer.color = Color.clear;
        }
        else if(spriteRenderer.color == Color.clear)
        {
            spriteRenderer.color = Color.white;
        }
    }
    public void ResetMark()
    {
        spriteRenderer.color = Color.white;
        isDrawn = false;

        drawScript.drawIndex = 0;
    }
}
