using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommunicationIcon : MonoBehaviour
{
    private bool canPickup;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // "Recycle" icons
        if(Input.GetKeyDown(KeyCode.Backspace) && canPickup)
        {
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
