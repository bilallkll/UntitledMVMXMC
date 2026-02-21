using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SymbolEvetns : MonoBehaviour
{
    public MarkerParent markerParent;
    public void Crossed()
    {
        Debug.Log("TryCross");
        if (markerParent.isTouchingWeakWall)
        {
            Debug.Log("Crosseeed");
            Destroy(markerParent.weakWall);
        }
    }
    public void SuperCrossed()
    {
        Debug.Log("Super Crossed");
    }
    public void SuperCrossedTwo()
    {
        Debug.Log("Super CrossedYwo");
    }

}
