using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float sizeOffset;
    public float smoothTime = 0.25f;
    public Vector3 offset = new Vector3(0f, 0f, -10f);
    
    private Vector3 velocity = Vector3.zero;

    void Start()
    {
        sizeOffset = GetComponent<Camera>().orthographicSize;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 targetPosition = target.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}
