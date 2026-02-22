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
    public InputActionReference controllerCursorInput;
    public float controllerSensibility;
    public float maxCursorDistanceFromCam;
    bool tooFarFromCam;
    [Header("Bool")]
    public bool isDrawing;
    bool checkedDrawing;
    bool isKeyboardAndMouse;
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
        ControllerAndMouseDetection();
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

    public void ControllerAndMouseDetection()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            isKeyboardAndMouse = true;
            Cursor.visible = false;
        }
        else if (Gamepad.current != null && (Gamepad.current.buttonSouth.wasPressedThisFrame || Gamepad.current.rightStick.ReadValue().magnitude > 0.1f))
        {
            isKeyboardAndMouse = false;
        }
         
    }

    public void CursorFollow()
    {
        if (isKeyboardAndMouse)
        {
            
            cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        else
        {
            cursorPos += controllerCursorInput.action.ReadValue<Vector2>() * controllerSensibility;
            float distance = Vector2.Distance(cursorPos, transform.position);
            if(maxCursorDistanceFromCam <= distance)
            {
                tooFarFromCam = true;
            }
            else
            {
                tooFarFromCam = false;
            }
            if(tooFarFromCam && cursorPos != (Vector2)cursor.position)
            {
                cursorPos = cursor.position;
            }
        }
        if (isDrawing)
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
