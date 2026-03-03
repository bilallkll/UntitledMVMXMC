using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SymbolEvetns : MonoBehaviour
{
    public MarkerParent markerParent;
    public PlayerController controller;
    public void Interact()
    {
        if (markerParent.isTouchingWeakWall)
        {
            Debug.Log("destroyedWall");
            Destroy(markerParent.weakWall);
        }
        if(markerParent.isTouchingIntro)
        {
            Debug.Log("intro");
            markerParent.introMan.ActivateIntro();
        }
        if (markerParent.isTouchingLever)
        {
            markerParent.leverController.ActivateLever();
        }
    }
    public void CreateTpBall()
    {
        controller.spawnPointApparition = true;
    }
    public void TpToBall()
    {
        controller.GoBackToSpawn();
    }

}
