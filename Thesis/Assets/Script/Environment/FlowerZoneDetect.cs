using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerZoneDetect : MonoBehaviour
{
    private PiggleCommunica piggleCommunica;

    private Animator flowerHealthAnim;
    private float piggleWaitTime;
    private bool startWaiting;

    void Start()
    {
        flowerHealthAnim = transform.GetChild(1).GetComponent<Animator>();
        flowerHealthAnim.enabled = false;

        piggleCommunica = GameObject.FindGameObjectWithTag("PiggleSign").GetComponent<PiggleCommunica>();
    }

    // Update is called once per frame
    void Update()
    {
        if(piggleWaitTime > 5f)
        {
            piggleCommunica.flowerPos = transform.position;
            piggleCommunica.needClearFlower = true;
        }
        else
        {
            if (startWaiting)
            {
                piggleWaitTime += Time.deltaTime;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            flowerHealthAnim.enabled = true;
        }

        if (collision.CompareTag("Newbie"))
        {
            startWaiting = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(UIFadeOut());
        }

        if (collision.CompareTag("Newbie"))
        {
            piggleWaitTime = 0f;
            startWaiting = false;
            piggleCommunica.taskSolved = true;
            piggleCommunica.needClearFlower = false;
        }
    }

    private IEnumerator UIFadeOut()
    {
        flowerHealthAnim.SetBool("FadeOut", true);
        yield return new WaitForSeconds(1f);
        flowerHealthAnim.SetBool("FadeOut", false);
        flowerHealthAnim.enabled = false;
    }
}
