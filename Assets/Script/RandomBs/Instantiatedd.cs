using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instantiatedd : MonoBehaviour
{
    public float actionTime;
    public bool destroy;
    void OnEnable()
    {
        StartCoroutine(destr());
    }

    IEnumerator destr()
    {
        yield return new WaitForSeconds(actionTime);
        if(destroy)
            Destroy(gameObject);
        else
            gameObject.SetActive(false);
    }
}
