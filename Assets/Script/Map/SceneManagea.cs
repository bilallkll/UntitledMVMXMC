using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagea : MonoBehaviour
{
    public float sceneTransitionTime = 1;
    public Animator fadeAnim;
    public PlayerController controller;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("SceneTransition"))
        {
            ChangeScene(collision.GetComponent<SceneTransitionHolder>().nextSceneName);
        }
    }

    public void ChangeScene(string sceneName)
    {
        //set last Scene
        PlayerPrefs.SetString("LastScene", SceneManager.GetActiveScene().name);
        fadeAnim.SetTrigger("Fade");
        controller.disableMovement = true;

        //next Scene
        StartCoroutine(SceneChangeDelay(sceneName));

    }
    IEnumerator SceneChangeDelay(string sceneName)
    {
        yield return new WaitForSeconds(sceneTransitionTime);
        //next Scene
        SceneManager.LoadScene(sceneName);
    }
}
