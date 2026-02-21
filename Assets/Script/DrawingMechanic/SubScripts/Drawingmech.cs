using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Drawingmech : MonoBehaviour
{
    public Transform cursor;
    public TrailRenderer trailEff;
    Vector2 cursorPos;
    Vector2 cursorStartPos;
    public GameObject marksParent;
    public Vector2 MaxMinHorizontal;
    public Vector2 MaxMinVertical;
    public MarkerParent markerParent;
    [Header("Input")]
    public InputActionReference drawInput;
    [Header("Bool")]
    public bool isDrawing;
    bool checkedDrawing;
    [Header("Combos")]
    public int drawIndex;
    public DrawMark[] dots;
    public SymbolPatern[] symbPatern;
    public UnityEvent[] symbEvents;



    private void Start()
    {
        Cursor.visible = false;
    }
    private void OnEnable()
    {
        drawInput.action.started += StartDrawmech;
    }
    private void OnDisable()
    {
        drawInput.action.started -= StartDrawmech;
    }

        
    void Update()
    {
        CursorFollow();
        //Draw when clicking
        if (drawInput.action.ReadValue<float>() > 0)
        {
            DrawMech();
        }
        else
        {
            UnDrawMech();
        }
    }


    public void CursorFollow()
    {
        cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if(isDrawing)
            cursorPos = new Vector2(Mathf.Clamp(cursorPos.x, cursorStartPos.x + 0, cursorStartPos.x + MaxMinHorizontal.y), 
                Mathf.Clamp(cursorPos.y, cursorStartPos.y + 0, cursorStartPos.y + MaxMinVertical.y));
        cursor.position = cursorPos;
    }


    public void StartDrawmech(InputAction.CallbackContext obj)
    {
        cursorStartPos = cursorPos;
        checkedDrawing = false;
        foreach (SymbolPatern sym in symbPatern)
        {
            sym.isThisSymbol = true;
        }
        dots[6].canBeNext = true;
        marksParent.SetActive(true);
        marksParent.transform.position = cursorPos;
    }
    public void DrawMech()
    {
        isDrawing = true;
        trailEff.enabled = true;
        trailEff.transform.position = cursorPos;
    }
    public void UnDrawMech()
    {
        isDrawing = false;
        trailEff.Clear();
        trailEff.enabled = false;
        if (!checkedDrawing)
        {
            MarkActivator();
            marksParent.SetActive(false);
            checkedDrawing = true;
        }
    }

    public void MarkCheck(GameObject mark)
    {
        //mark cleaner
        foreach (DrawMark dot in dots)
        {
            dot.solutionNumber = 0;
            dot.canBeNext = false;
        }
        foreach (SymbolPatern sym in symbPatern)
        {
            if (drawIndex < sym.symbolPatern.Length)
            {
                if (dots[sym.symbolPatern[drawIndex] - 1].gameObject != mark.gameObject)
                {
                    sym.isThisSymbol = false;

                }
                else
                {

                    
                    //only let the next possible dot and this dot active

                    dots[sym.symbolPatern[drawIndex] - 1].canBeNext = true;
                    if(drawIndex+1 < sym.symbolPatern.Length)
                    {
                        foreach (DrawMark dot in dots)
                        {
                            if (dot.canBeNext == false)
                            {
                                dot.solutionNumber++;
                            }
                        }
                        dots[sym.symbolPatern[drawIndex + 1] - 1].canBeNext = true;
                        Debug.Log(dots[sym.symbolPatern[drawIndex] - 1]);
                    }

                }
            }
            else
            {
                sym.isThisSymbol = false;
            }


        }
            
        drawIndex++;
        
        
    }
    public void MarkActivator()
    {

        foreach (SymbolPatern sym in symbPatern) 
        {
            if (sym.isThisSymbol && drawIndex == sym.symbolPatern.Length)
            {
                symbEvents[sym.symbolEvent].Invoke();
            }
            sym.isThisSymbol = false;
        }

        foreach(Transform mark in markerParent.transform)
        {
            mark.GetComponent<DrawMark>().ResetMark();
        }
    }


    


}
