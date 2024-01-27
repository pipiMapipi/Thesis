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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //SceneManager.LoadScene("combat");



        circleAnim.SetTrigger("Start");
        //circleMask.SetActive(false);
    }
}
