using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewbieAttack : MonoBehaviour
{
    private GameObject armor;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Shielded"))
        {
            armor = collision.transform.gameObject.transform.GetChild(0).gameObject;
            armor.SetActive(false);
        }
    }

}
