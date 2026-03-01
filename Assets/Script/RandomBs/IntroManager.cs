using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroManager : MonoBehaviour
{
    public Animator lightAnim;
    public PlayerController controller;
    public float ableMovementDelay;

    void Start()
    {
        if(PlayerPrefs.GetInt("intro") == 0)
        {
            controller.disableMovement = true;
            PlayerPrefs.SetInt("intro", 1);
        }
        else
        {
            lightAnim.SetTrigger("lightsOn");

        }
    }

    public void ActivateIntro()
    {
        lightAnim.SetTrigger("lightsOn");
        StartCoroutine(moveDelay());
    }
    IEnumerator moveDelay()
    {
        
        yield return new WaitForSeconds(ableMovementDelay);
        controller.disableMovement = false;
    }
}
