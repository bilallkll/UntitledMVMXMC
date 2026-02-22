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
    float treshold;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    void Update()
    {
        float distance = Vector2.Distance(cursor.transform.position, transform.position); 
        if(!canBeNext)
        {
            treshold = .1f;
            spriteRenderer.color = Color.gray;
        }
        else
        {

            spriteRenderer.color = Color.white;
            treshold = globalVar.drawMarkTreshold[solutionNumber];
        }
        if (distance < treshold  && drawScript.isDrawing && !isDrawn)
        {

            spriteRenderer.color = Color.red;
            isDrawn = true;

            drawScript.MarkCheck(gameObject);
        }
        else if (drawScript.isDrawing == false && isDrawn)
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
