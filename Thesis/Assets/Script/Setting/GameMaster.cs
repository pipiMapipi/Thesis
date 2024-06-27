using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{
    public Vector2 lastCheckPointPos;
    public Vector2 playerZoomPos;


    [Header("Transition Anim")]
    [SerializeField] private Animator circleAnim;
    [SerializeField] private GameObject circleMask;


    public static GameMaster instance;

    [Header("Audio")]
    [SerializeField] private List<AudioClip> audioClip = new List<AudioClip>();
    private AudioSource audioNow;

    [Header("Score Track")]
    public static int slimeSum;
    public static int slimePlayer;
    public static int slimePiggle;
    public static float slimePlayerRatio;
    public static float slimePiggleRatio;
    public static int potionCanDrop;
    public static int potionPlayer;
    public static int potionPiggle;
    public static float potionPlayerRatio;
    public static float potionPiggleRatio;
    public int slimeSum0;
    public int slimePlayer0;
    public  int slimePiggle0;
    public  int potionPlayer0;
    public  int potionPiggle0;
    public int potionCanDrop0;

    public static bool needCommunication;
    public static bool needSlime;
    public static bool needJump;
    public static bool needFountain;
    public static bool needBridge;

    public static int deathTime;

    [Header("Conversation")]
    [SerializeField] private GameObject communication;
    [SerializeField] private GameObject communicationIcon;
    public static bool commTriggered;

    public static bool machineIsGone;
    private void Awake()
    {
        //if (instance == null)
        //{
        //    instance = this;
        //    DontDestroyOnLoad(instance);
        //}
        //else
        //{
        //    Destroy(gameObject);
        //}

        //playerHealth = player.GetComponent<DamageableCharacter>();
        audioNow = GetComponent<AudioSource>();
        AudioControl();

        if (SceneManager.GetActiveScene().name == "combat")
        {
            slimeSum = GameObject.FindGameObjectWithTag("SlimeTeam").transform.childCount;
            
            slimePlayer = 0;
            slimePiggle = 0;
            potionPlayer = 0;
            potionPiggle = 0;
            potionCanDrop = 0;

            foreach (GameObject slime in GameObject.FindGameObjectsWithTag("SlimePrefab"))
            {
                if (slime.GetComponent<HealthPotionTrigger>().canDrop)
                {
                    potionCanDrop++;
                }
            }
        }
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "combat")
        {
            foreach (GameObject slime in GameObject.FindGameObjectsWithTag("SlimePrefab"))
            {
                if (slime.GetComponent<HealthPotionTrigger>().canDrop)
                {
                    potionCanDrop++;
                }
            }
        }
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Home");
            deathTime = 0;
            commTriggered = false;
            needFountain = false;
            needBridge = false;
            needJump = false;
        }

        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (!audioNow.isPlaying)
        {
            AudioControl();
            audioNow.Play();
        }

        TrackCombat();
        

        slimePiggle0 = slimePiggle;
        slimeSum0 = slimeSum;
        slimePlayer0 = slimePlayer;
        potionPiggle0 = potionPiggle;
        potionPlayer0 = potionPlayer;
        potionCanDrop0 = potionCanDrop;

        if (SceneManager.GetActiveScene().name == "Score")
        {
            slimePiggleRatio = slimePiggle0 / slimeSum0;
            slimePlayerRatio = slimePlayer0 / slimeSum0;
            potionPiggleRatio = potionPiggle0 / potionCanDrop0;
            potionPlayerRatio = potionPlayer0 / 4f;
        }
    }

    public void DeathReset()
    {
        StartCoroutine(TransitionAnim());
    }

    //private IEnumerator DeathRestart()
    //{
    //    if (playerHealth.Health == 0)
    //    {
    //        yield return new WaitForSeconds(2f);

    //        if(lastCheckPointPos != null)
    //        {
                
    //            StartCoroutine(TransitionAnim());
    //        }
    //        else
    //        {
    //            Debug.Log("No Check Point");
    //        }
    //    }
    //}

    private IEnumerator TransitionAnim()
    {
        //circleMask.SetActive(true);
        circleAnim.SetTrigger("End");
        yield return new WaitForSeconds(1);

        //player.transform.position = lastCheckPointPos;
        //yield return new WaitForSeconds(1f);

        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //playerZoomPos = lastCheckPointPos;
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        deathTime++;
        SceneManager.LoadScene("Score");



        circleAnim.SetTrigger("Start");
        //circleMask.SetActive(false);
    }

    private void AudioControl()
    {
        if (SceneManager.GetActiveScene().name != "combat")
        {
            audioNow.clip = audioClip[0];
        }
        else
        {
            audioNow.clip = audioClip[1];
            
        }
    }

    public void NextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ChangeToCombat()
    {
        SceneManager.LoadScene("combat");
    }

    public void ChangeToReborn()
    {
        SceneManager.LoadScene("Reborn");
    }

    private void TrackCombat()
    {
        // Communication
        if(deathTime == 1)
        {
            needCommunication = true;
            commTriggered = true;
        }else
        {
            needCommunication = false;
            
        }

        if (SceneManager.GetActiveScene().name == "combat")
        {
            if (commTriggered)
            {
                communication.SetActive(true);
                communicationIcon.SetActive(true);
            }
            else
            {
                communication.SetActive(false);
                communicationIcon.SetActive(false);
            }
        }

        // Slime
        if (deathTime >= 1)
        {
            needSlime = true;
        }else
        {
            needSlime = false;
        }

        // Fountain

    }
}
