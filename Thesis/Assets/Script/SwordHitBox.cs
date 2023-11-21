using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordHitBox : MonoBehaviour
{

    [SerializeField] private float swordDamage = 1f;
    [SerializeField] private float knockbackForce = 500f;

    private Collider2D swordCollider;
   
    void Start()
    {
        swordCollider = gameObject.GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster"))
        {
            // check if there is IDamageable in the script of the gameobject
            IDamageable damageableObject = collision.GetComponent<IDamageable>();
            if (damageableObject != null)
            {
                // Calculate direction between avatar and monsters
                Vector3 parentPos = transform.parent.position;
                Vector2 direction = (Vector2)(collision.gameObject.transform.position - parentPos).normalized;
                Vector2 knockback = direction * knockbackForce;

                //collision.SendMessage("OnHit", swordDamage, knockback);
                damageableObject.OnHit(swordDamage, knockback);
            }
            else
            {
                Debug.LogWarning("Collider does not implement IDmamageable");
            }
            
        }
        
        
    }
}

