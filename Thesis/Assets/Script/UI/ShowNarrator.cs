using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowNarrator : MonoBehaviour
{
    [SerializeField] private float initPosX;
    [SerializeField] private Transform player;
    //[SerializeField] private Canvas[] narrator;

    //public Canvas narrator1;
    //public Canvas narrator2;
    //public Canvas narrator3;
    public Canvas narrator;

    private bool narratorActivated;
    private int textNow = 0;
    private bool isUpdating;


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (player.position.x > initPosX)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                narratorActivated = true;
                
            }
        }

        //if (narratorActivated && !narrator3.gameObject.activeSelf)
        //{
        //    StartCoroutine(NarratorSpeak());
           
            
        //}

        if (narratorActivated)
        {
            narrator.gameObject.SetActive(true);


        }
    }

    //IEnumerator NarratorSpeak()
    //{
    //    //narrator[sequence].gameObject.SetActive(true);

    //    //if (sequence > 0)
    //    //{
    //    //    narrator[sequence-1].gameObject.SetActive(false);
    //    //}

    //    //yield return new WaitForSeconds(3f);
    //    //isUpdating = false;
    //    //StartCoroutine(NextText());

    //    narrator1.gameObject.SetActive(true);
    //    yield return new WaitForSeconds(3f);

    //    narrator1.gameObject.SetActive(false);
    //    narrator2.gameObject.SetActive(true);
    //    yield return new WaitForSeconds(3f);

    //    narrator2.gameObject.SetActive(false);
    //    narrator3.gameObject.SetActive(true);
    //    yield return new WaitForSeconds(3f);
    //}

    IEnumerator NextText()
    {
        if (!isUpdating)
        {
            textNow++;
            isUpdating = true;
        }
        
        yield return null;
        
    }
}
