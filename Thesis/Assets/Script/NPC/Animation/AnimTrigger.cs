using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AnimTrigger : MonoBehaviour
{
    [SerializeField] private float zoomInSize = 2.6f;

    private Camera cam;
    private CameraFollow cameraFollow;
    private Animator anim;
    private Animator playerAnim;
    private GameObject player;
    private float sizeDefault;

    private bool camStart;
    private float changeSpeed;
    private int lerpDir;

    private AnimEvent animEvent;
    private Animator cinemaEffect;

    void Start()
    {
        anim = transform.parent.transform.GetComponent<Animator>();
        anim.enabled = false;

        player = GameObject.FindGameObjectWithTag("Player");
        playerAnim = player.GetComponent<Animator>();

        cam = Camera.main;
        cameraFollow = cam.GetComponent<CameraFollow>();
        sizeDefault = cam.GetComponent<Camera>().orthographicSize;

        animEvent = transform.parent.transform.GetComponent<AnimEvent>();
        cinemaEffect = GameObject.FindGameObjectWithTag("CinemaEffect").GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(camStart)
        {
            changeSpeed += Time.deltaTime * lerpDir;
            cam.GetComponent<Camera>().orthographicSize = Mathf.Lerp(sizeDefault, zoomInSize, changeSpeed);
            if(changeSpeed > 1f)
            {
                camStart = false;
                changeSpeed = 1f;
            } else if (changeSpeed < 0f)
            {
                camStart = false;
                changeSpeed = 0f;
            }
        }

        if (animEvent.animEnd)
        {
            StartCoroutine(SetCamToDefault());
        }
            
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerAnim.SetFloat("Horizontal", 0f);
            playerAnim.SetFloat("Vertical", 0f);
            playerAnim.SetFloat("Speed", 0f);
            player.GetComponent<PlayerMovement>().enabled = false;
            player.GetComponent<PlayerInput>().enabled = false;

            StartCoroutine(InitCam());
            StartCoroutine(StartAnimation());
        }
    }

    private IEnumerator StartAnimation()
    {
        yield return new WaitForSeconds(2f);
        anim.enabled = true;
    }

    private IEnumerator InitCam()
    {
        yield return new WaitForSeconds(1.5f);
        cinemaEffect.SetBool("InitVFX", true);
        cameraFollow.target = transform.parent.transform;
        cameraFollow.offset = new Vector3(0f, -0.1f, -10f);
        yield return new WaitForSeconds(cameraFollow.smoothTime);
        if(cam.GetComponent<Camera>().orthographicSize > zoomInSize)
        {
            camStart = true;
            lerpDir = 1;
        }
        
    }

    private IEnumerator SetCamToDefault()
    {
        yield return new WaitForSeconds(0.7f);
        
        if (cam.GetComponent<Camera>().orthographicSize < sizeDefault)
        {
            camStart = true;
            lerpDir = -1;
        }
        yield return new WaitForSeconds(1f);
        cameraFollow.target = player.transform;
        cameraFollow.offset = new Vector3(0f, 0f, -10f);
        yield return new WaitForSeconds(cameraFollow.smoothTime);
        cinemaEffect.SetBool("InitVFX", false);
        player.GetComponent<PlayerMovement>().enabled = true;
        player.GetComponent<PlayerInput>().enabled = true;

    }

    
}
