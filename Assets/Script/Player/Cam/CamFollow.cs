using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    [Header("Component")]
    public Transform target;
    public PlayerController controller;

    [Header("Dont Touch Value")]
    public float smoothTime = 0.3F;
    private Vector3 velocity = Vector3.zero;
    public float Yoffset = 5;
    [Header("Changeable Value")]
    public Vector2 XoffsetLookAt;
    public Vector2 maxHorizontal;
    public Vector2 maxVertical;

    Vector3 targetPosition;
    [Header("bools")]
    public bool follow = true;
    bool fixedFirst;
    [HideInInspector] public bool followingPlayer;
    [HideInInspector] public bool newSpace;


    private void Start()
    {
        StartCoroutine(delay());
    }
    IEnumerator delay()
    {
        if (follow) 
        { 
            transform.position = target.TransformPoint(new Vector3(0, Yoffset, -10));
            followingPlayer = true;
        }
        yield return new WaitForEndOfFrame();
        fixedFirst = true;
    }
    void FixedUpdate()
    {
        if (!follow || !fixedFirst) return;
        transform.position = Vector3.SmoothDamp(transform.position, targetPos(), ref velocity, smoothTime);
    }

    public Vector3 targetPos()
    {
        float Xoffset;
        Vector3 tarogeto;

        //Clamp Y
        if(followingPlayer)
            tarogeto.y = target.TransformPoint(new Vector3(0, Yoffset, 0)).y;
        else
            tarogeto.y = target.TransformPoint(new Vector3(0, 0, 0)).y;
        tarogeto.y = Mathf.Clamp(tarogeto.y, maxVertical.x, maxVertical.y);

        //Clamp X
        if (followingPlayer)
        {
            Xoffset = controller._facingRight ? XoffsetLookAt.x : XoffsetLookAt.y;
            tarogeto.x = target.TransformPoint(new Vector3(Xoffset, 0, 0)).x;
        }
        else
            tarogeto.x = target.TransformPoint(new Vector3(0, 0, 0)).x;
        tarogeto.x = Mathf.Clamp(tarogeto.x, maxHorizontal.x, maxHorizontal.y);

        tarogeto.z = -10;

        return tarogeto;
    }
}
