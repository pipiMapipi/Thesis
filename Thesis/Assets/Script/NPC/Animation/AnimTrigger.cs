using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AnimTrigger : MonoBehaviour
{
    [SerializeField] private float zoomInSize = 2.6f;
    [SerializeField] private List<int> emotionIndex = new List<int>();

    [Header("Fullscreen Animation")]
    [SerializeField] private bool hasFSAnim;
    [SerializeField] private List<GameObject> FSAnim = new List<GameObject>();

    [Header("Transition Animation")]
    [SerializeField] private bool hasTransitAnim;
    [SerializeField] private GameObject transitAnim;

    [Header("Add to Scene")]
    [SerializeField] private bool hasNewObject;
    [SerializeField] private List<GameObject> newObjects = new List<GameObject>();


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

    private GameObject emotionCanvas;
    private EmotionUI emotionUI;

    private bool FSAnimPlayed = false;
    private bool FSAnimEnded;
    private int currentAnimIndex = 0;
    private bool playFSAnimsTrigger = false;

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

        emotionCanvas = GameObject.FindGameObjectWithTag("Emotion");
        emotionUI = GameObject.FindGameObjectWithTag("Emotion").GetComponent<EmotionUI>();
        for(int i = 0; i < emotionCanvas.transform.childCount; i++)
        {
            emotionCanvas.transform.GetChild(i).gameObject.SetActive(false);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (hasTransitAnim)
        {
            if (transitAnim.GetComponent<TransitAnimation>().transitAnimEnd)
            {
                if (!FSAnimPlayed) playFSAnimsTrigger = true;
            }

            if (transitAnim.GetComponent<TransitAnimation>().camOverwrite) return;

        }

        

        if (camStart)
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

        

        

        if (playFSAnimsTrigger)
        {
            PlayFSAnims();
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
            player.GetComponent<PlayerMovement>().stopMovementInput = true;
            player.GetComponent<PlayerMovement>().movement = Vector2.zero;
            
            
            //playerAnim.SetFloat("Horizontal", 0f);
            //playerAnim.SetFloat("Vertical", 0f);
            //playerAnim.SetFloat("Speed", 0f);
            
            player.GetComponent<PlayerInput>().enabled = false;


            for (int i = 0; i < emotionCanvas.transform.childCount; i++)
            {
                emotionCanvas.transform.GetChild(i).gameObject.SetActive(true);
            }
            emotionUI.CurrentEmotion(emotionIndex[0]);
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

        emotionUI.CurrentEmotion(emotionIndex[1]);
        if (!hasTransitAnim || (hasTransitAnim && !transitAnim.GetComponent<TransitAnimation>().camOverwrite))
        {
            cameraFollow.target = player.transform;
            cameraFollow.offset = new Vector3(0f, 0f, -10f);
        } 
        
        yield return new WaitForSeconds(cameraFollow.smoothTime);
        if (cam.GetComponent<Camera>().orthographicSize < sizeDefault)
        {
            camStart = true;
            lerpDir = -1;
        }
        yield return new WaitForSeconds(1f);
        

        if (!hasFSAnim)
        {
            cinemaEffect.SetBool("InitVFX", false);
            yield return new WaitForSeconds(0.7f);
            emotionUI.AnimFadeOut();
            yield return new WaitForSeconds(0.3f);
            for (int i = 0; i < emotionCanvas.transform.childCount; i++)
            {
                emotionCanvas.transform.GetChild(i).gameObject.SetActive(false);
            }

            player.GetComponent<PlayerMovement>().stopMovementInput = false;
            player.GetComponent<PlayerInput>().enabled = true;
            Destroy(gameObject);
        }
        else
        {
            if (!FSAnimEnded)
            {
                emotionUI.AnimFadeOut();
                yield return new WaitForSeconds(0.3f);
                for (int i = 0; i < emotionCanvas.transform.childCount; i++)
                {
                    emotionCanvas.transform.GetChild(i).gameObject.SetActive(false);
                }
                yield return new WaitForSeconds(1f);
                if (hasTransitAnim)
                {
                    transitAnim.SetActive(true);
                }
                else
                {
                    if (!FSAnimPlayed) playFSAnimsTrigger = true;
                }
                
            }
            else
            {
                if (hasNewObject) AddNewObjectToScene();
                yield return new WaitForSeconds(1f);
                cinemaEffect.SetBool("InitVFX", false);
                yield return new WaitForSeconds(1f);

                player.GetComponent<PlayerMovement>().stopMovementInput = false;
                player.GetComponent<PlayerInput>().enabled = true;
                
                Destroy(transitAnim);
                Destroy(gameObject);
            }
            
            
        }
        
    }


    private void PlayFSAnims()
    {
        
        FSAnimPlayed = true;
        if (currentAnimIndex >= FSAnim.Count)
        {
            FSAnimEnded = true;            
            return;
        }

        FSAnim[currentAnimIndex].SetActive(true);
        AnimEvent animEvent = FSAnim[currentAnimIndex].GetComponent<AnimEvent>();
        if (animEvent.animEnd)
        {
            FSAnim[currentAnimIndex].SetActive(false);
            currentAnimIndex++;
            FSAnimPlayed = false;
        } 
    }

    private void AddNewObjectToScene()
    {
        foreach(GameObject newObject in newObjects)
        {
            newObject.SetActive(true);
        }
    }

}
