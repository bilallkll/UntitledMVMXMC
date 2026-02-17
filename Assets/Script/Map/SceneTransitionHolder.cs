using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.SceneManagement;

public class SceneTransitionHolder : MonoBehaviour
{
    public string nextSceneName;
    public string lastSceneName;
    public Transform spawnPos; 
    public float sceneTransitionTime = 1;
    public bool facingRight = true;
    

    private void OnEnable()
    {
        PlayerController controller = GameObject.Find("Player").GetComponent<PlayerController>();
        controller.disableMovement = true;
        controller._facingRight = facingRight;
        StartCoroutine(Delay());
        if (PlayerPrefs.GetString("LastScene") == lastSceneName)
        {
            GameObject.Find("Player").transform.position = spawnPos.position;
        }
    }
    IEnumerator Delay()
    {
        yield return new WaitForSeconds(sceneTransitionTime);
        GameObject.Find("Player").GetComponent<PlayerController>().disableMovement = false;
    }
}
