using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordHitBox : MonoBehaviour
{
    public float swordDamage = 1f;

    private Collider2D swordCollider;
   
    void Start()
    {
        swordCollider = gameObject.GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.collider.CompareTag("Monster"))
    //    {
    //        collision.collider.SendMessage("OnHit", swordDamage);
    //    }
    //}


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster"))
        {
            collision.SendMessage("OnHit", swordDamage);
        }
    }
}

