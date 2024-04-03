using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedCollider : MonoBehaviour
{
    private FakeHeight fakeHeight;
    private Transform newbie;

    private List<Collider2D> enemyList = new List<Collider2D>();

    void Start()
    {
        fakeHeight = transform.parent.transform.GetComponent<FakeHeight>();
        newbie = GameObject.FindGameObjectWithTag("Newbie").transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster"))
        {
            enemyList.Add(collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster"))
        {
            enemyList.Remove(collision);
        }
    }

    private void AttackEnemy()
    {
        Debug.Log(fakeHeight.applyDamage);
        //foreach (enemy) in enemyList {

        //}
        //if (collision.CompareTag("Monster") && fakeHeight.applyDamage)
        //{

        //    IDamageable damageableObject = collision.GetComponent<IDamageable>();
        //    if (damageableObject != null)
        //    {
        //        // Calculate direction between avatar and monsters
        //        Vector3 parentPos = transform.parent.position;
        //        Vector2 direction = (Vector2)(collision.gameObject.transform.position - parentPos).normalized;
        //        Vector2 knockback = direction * fakeHeight.knockbackForce;

        //        //collision.SendMessage("OnHit", swordDamage, knockback);
        //        damageableObject.OnHit(fakeHeight.seedDamage, knockback);

        //        float enemyHealth = collision.transform.GetComponent<DamageableCharacter>().Health;
        //        if (enemyHealth == 0)
        //        {
        //            newbie.GetComponent<DamageableCharacter>().Aggro += 1.5f;
        //        }
        //    }
        //    else
        //    {
        //        Debug.LogWarning("Collider does not implement IDmamageable");
        //    }
        //}
    }
}
