using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommunicationIcon : MonoBehaviour
{
    private bool canPickup;
    private CommunictionIconTrigger communictionIconTrigger;
    void Start()
    {
        if(transform.parent.CompareTag("PlayerSign")) communictionIconTrigger = transform.parent.GetComponent<CommunictionIconTrigger>();

    }

    // Update is called once per frame
    void Update()
    {
        // "Recycle" icons
        if((Input.GetKeyDown(KeyCode.Delete) && canPickup) || ((transform.parent.CompareTag("PlayerSign")) && communictionIconTrigger.clearSign))
        {
            communictionIconTrigger.signDropped = false;
            communictionIconTrigger.piggleWaitTime = 0f;
            communictionIconTrigger.clearSign = false;
            gameObject.SetActive(false);
            
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canPickup = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canPickup = false;
        }
    }
}
