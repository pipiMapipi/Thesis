using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitAnimation : MonoBehaviour
{
    public Vector2 targetPos;
    public float moveSpeed;
    public float zoomInSize;
    public bool transitAnimEnd;
    public Transform camPanEndPos;

    private GameObject player;
    private Rigidbody2D rb;
    private Vector2 direction;
    private Animator anim;

    private Camera cam;
    private CameraFollow cameraFollow;
    private float sizeDefault;
    private bool camZoomStart;
    private float changeSpeed;
    private int lerpDir;


    private bool transitStarted;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = player.GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        anim.enabled = false;

        cam = Camera.main;
        cameraFollow = cam.GetComponent<CameraFollow>();
        sizeDefault = cam.GetComponent<Camera>().orthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        MoveToPosition();
        if(Vector2.Distance(transform.position, targetPos) < 0.1f && !transitStarted)
        {
            StartCoroutine(InitTransitionAnim());
        }

        if (camZoomStart)
        {
            changeSpeed += Time.deltaTime * lerpDir;
            cam.GetComponent<Camera>().orthographicSize = Mathf.Lerp(sizeDefault, zoomInSize, changeSpeed);
            if (changeSpeed > 1f)
            {
                camZoomStart = false;
                changeSpeed = 1f;
            }
            else if (changeSpeed < 0f)
            {
                camZoomStart = false;
                changeSpeed = 0f;
            }
        }
    }

    private void MoveToPosition()
    {
        direction = (targetPos - (Vector2)transform.position).normalized;
        rb.AddForce(direction * moveSpeed * Time.deltaTime);
    }

    private IEnumerator InitTransitionAnim()
    {
        transitStarted = true;
        yield return new WaitForSeconds(0.5f);

        // Cam switch target
        cameraFollow.target = transform;
        yield return new WaitForSeconds(0.7f);

        // Cam Zoom in on target
        camZoomStart = true;
        lerpDir = 1;
        yield return new WaitForSeconds(2f);

        // Play target anim
        anim.enabled = true;
        yield return new WaitForSeconds(0.2f);
        // Camera pan
        cameraFollow.target = camPanEndPos;
        yield return new WaitForSeconds(0.5f);
        transitAnimEnd = true;
    }
}
