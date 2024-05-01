using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitAnimation : MonoBehaviour
{
    [Header("Move Player")]
    public Transform targetPos;
    public float moveSpeed;
    [Header("Camera Movement")]
    public float zoomInSize;
    public Transform camPanEndPos;
    [SerializeField] private Animator focusObjectAnim;
    [Header("Inform AnimTrigger")]
    public bool transitAnimEnd;
    public bool camOverwrite;

    private GameObject player;
    private Animator anim;

    private Camera cam;
    private CameraFollow cameraFollow;
    private float sizeDefault;
    private bool camZoomStart;
    private float changeSpeed;
    private int lerpDir;
    private float playerSpeedDefault;


    private bool transitStarted;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerSpeedDefault = player.GetComponent<PlayerMovement>().PlayerSpeed;
        anim = GetComponent<Animator>();
        anim.enabled = false;

        cam = Camera.main;
        cameraFollow = cam.GetComponent<CameraFollow>();
        sizeDefault = cam.GetComponent<Camera>().orthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        if(!transitAnimEnd) MoveToPosition();
        if (Vector2.Distance(player.transform.position, targetPos.position) < 0.1f)
        {
            player.transform.position = targetPos.position;
            if (!transitStarted) StartCoroutine(InitTransitionAnim());
            
        }
        else
        {
            
            player.GetComponent<PlayerMovement>().animControll = true;
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
        player.GetComponent<PlayerMovement>().moveDir = (targetPos.position - player.transform.position).normalized;
        player.GetComponent<PlayerMovement>().PlayerSpeed = moveSpeed;
        //rb.AddForce(direction * moveSpeed * Time.deltaTime);

    }

    private IEnumerator InitTransitionAnim()
    {
        //player.GetComponent<PlayerMovement>().animControll = false;
        //player.GetComponent<PlayerMovement>().PlayerSpeed = 0f;
        //player.GetComponent<PlayerMovement>().movement = Vector2.zero;
        transitStarted = true;
        yield return new WaitForSeconds(0.5f);

        // Cam switch target
        camOverwrite = true;
        cameraFollow.target = transform;
        cameraFollow.offset = new Vector3(0f, -0.4f, -10f);

        yield return new WaitForSeconds(0.7f);

        // Cam Zoom in on target
        lerpDir = 1;
        camZoomStart = true;
        focusObjectAnim.enabled = true;
        yield return new WaitForSeconds(2f);

        // Play target anim
        anim.enabled = true;
        yield return new WaitForSeconds(0.5f);
        // Camera pan
        
        cameraFollow.target = camPanEndPos;
        yield return new WaitForSeconds(0.4f);
        camOverwrite = false;
        player.GetComponent<PlayerMovement>().animControll = false;
        focusObjectAnim.enabled = false;
        player.GetComponent<PlayerMovement>().PlayerSpeed = playerSpeedDefault;
        transitAnimEnd = true;
    }
}
