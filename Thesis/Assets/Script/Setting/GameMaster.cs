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

    [Header("Conversation")]
    public bool communication;
    public bool slim;

    [Header("Audio")]
    [SerializeField] private List<AudioClip> audioClip = new List<AudioClip>();
    private AudioSource audioNow;

    [Header("Score Track")]
    public static int slimeSum;
    public static int slimePlayer;
    public static int slimePiggle;
    public static int potionPlayer;
    public static int potionPiggle;
    public int slimeSum0;
    public int slimePlayer0;
    public  int slimePiggle0;
    public  int potionPlayer0;
    public  int potionPiggle0;

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
        }
    }
    private void Update()
    {
        if (!audioNow.isPlaying)
        {
            AudioControl();
            audioNow.Play();
        }

        if (SceneManager.GetActiveScene().name == "combat") TrackCombat();

        slimePiggle0 = slimePiggle;
        slimeSum0 = slimeSum;
        slimePlayer0 = slimePlayer;
        potionPiggle0 = potionPiggle;
        potionPlayer0 = potionPlayer;
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
        SceneManager.LoadScene("score");



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

    public void ChangeToCombat()
    {
        SceneManager.LoadScene("combat");
    }

    private void TrackCombat()
    {

    }
}
