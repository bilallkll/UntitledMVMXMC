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
    public DrawMark[] cross;
    public bool isCross;
    public DrawMark[] roof;
    public bool isRoof;



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
        isCross = true;
        isRoof = true;
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

        if (drawIndex < cross.Length)
        {
            if (cross[drawIndex].gameObject != mark.gameObject)
            {
                isCross = false;

            }
        }
        else
        {
            isCross = false;
        }


        if (drawIndex < roof.Length)
        {
            if (roof[drawIndex].gameObject != mark.gameObject)
            {
                isRoof = false;

            }
        }
        else
        {
            isRoof = false;
        }
            
        drawIndex++;
        
        
    }
    public void MarkActivator()
    {

        if(isCross && drawIndex == cross.Length)
        {
            CrossedEffect();
        }
        else if(isRoof && drawIndex == roof.Length)
        {
            Debug.Log("Roofed");
        }




        foreach(Transform mark in markerParent.transform)
        {
            mark.GetComponent<DrawMark>().ResetMark();
        }
        isCross = false;
        isRoof = false;
    }


    public void CrossedEffect()
    {
        Debug.Log("TryCross");
        if (markerParent.isTouchingWeakWall)
        {
            Debug.Log("Crosseeed");
            Destroy(markerParent.weakWall);
        }
    }


}
