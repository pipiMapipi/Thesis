using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{

    [Header("Attack")]
    [SerializeField] private float damage = 1f;
    [SerializeField] private float knockbackForce = 100f;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        IDamageable damageable = collision.collider.GetComponent<IDamageable>();

        if (collision.collider.CompareTag("Player"))
        {
            if (damageable != null)
            {
                Vector2 direction = (Vector2)(collision.gameObject.transform.position - transform.position).normalized;
                Vector2 knockback = direction * knockbackForce;

                //collision.SendMessage("OnHit", swordDamage, knockback);
                damageable.OnHit(damage);
            }
            else
            {
                Debug.LogWarning("Collider does not implement IDmamageable");
            }

        }
    }
}
