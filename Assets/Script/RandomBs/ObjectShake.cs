using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectShake : MonoBehaviour
{
    // Transform of the camera to shake. Grabs the gameObject's transform 
    // if null. 
    public Transform objectTransform;

    // How long the object should shake for. 
    public float shakeDuration = 0f;
    float originalShakeDuration;

    // Amplitude of the shake. A larger value shakes the camera harder. 
    public Vector2 shakeAmount = new Vector2(0.5f, 0.5f); 
    public float decreaseFactor = 1.0f;

    Vector3 originalPos;
    public bool shake;

    void Awake()
    {
        originalShakeDuration = shakeDuration;
        if (objectTransform == null)
        {
            objectTransform = GetComponent(typeof(Transform)) as Transform;
        }
    }

    void OnEnable()
    {
        originalPos = objectTransform.localPosition;
    }

    void Update()
    {
        if (shake)
        {
            if (shakeDuration > 0)
            {
                float x = originalPos.x + Random.Range(-1f, 1f) * shakeAmount.x;
                float y = originalPos.y + Random.Range(-1f, 1f) * shakeAmount.y;
                objectTransform.localPosition = new Vector3(x, y, originalPos.z);
                shakeDuration -= Time.deltaTime * decreaseFactor;
            }
            else
            {
                shakeDuration = originalShakeDuration;
                objectTransform.localPosition = originalPos;
                shake = false;
            }
        }
    }
}
