using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerZoneDetect : MonoBehaviour
{
    private Animator flowerHealthAnim;
    void Start()
    {
        flowerHealthAnim = transform.GetChild(1).GetComponent<Animator>();
        flowerHealthAnim.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            flowerHealthAnim.enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(UIFadeOut());
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
