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
    private bool cameraFullFollow;
    //private List<CamBorder> camBorder = new List<CamBorder>();

    private DamageableCharacter piggleHealth;

    void Start()
    {
        cameraFullFollow = true;
        sizeOffset = GetComponent<Camera>().orthographicSize;

        piggleHealth = GameObject.FindGameObjectWithTag("Newbie").GetComponent<DamageableCharacter>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (cameraFullFollow)
        {
            CompleteFollow();
        }
        else
        {
            VerticalFollowOnly();
        }

        if(piggleHealth.Health <= 0)
        {
            target = piggleHealth.gameObject.transform;
        }
    }

    private void Update()
    {
        foreach (GameObject camBorder in GameObject.FindGameObjectsWithTag("CamBorder"))
        {
            if (camBorder.GetComponent<CamBorder>().crossBorder)
            {
                camBorder.GetComponent<CamBorder>().crossBorder = false;
                cameraFullFollow = (cameraFullFollow) ? false : true;
            }
        }
    }

    private void CompleteFollow()
    {
        Vector3 targetPosition = target.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }

    private void VerticalFollowOnly()
    {
        Vector3 targetPosition = new Vector3(transform.position.x, target.position.y, target.position.z) + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }

}
