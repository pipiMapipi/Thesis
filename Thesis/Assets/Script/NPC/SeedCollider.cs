using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedCollider : MonoBehaviour
{
    private FakeHeight fakeHeight;
    private Transform newbie;

    private List<Collider2D> enemyList = new List<Collider2D>();
    private bool underAttack;

    void Start()
    {
        fakeHeight = transform.parent.transform.GetComponent<FakeHeight>();
        newbie = GameObject.FindGameObjectWithTag("Newbie").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (fakeHeight.applyDamage) AttackEnemy();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster") && !underAttack)
        {
            enemyList.Add(collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster") && !underAttack)
        {
            enemyList.Remove(collision);
        }
    }

    private void AttackEnemy()
    {
        foreach (Collider2D enemy in enemyList) {
            underAttack = true;
            IDamageable damageableObject = enemy.GetComponent<IDamageable>();
            Slime slime = enemy.GetComponent<Slime>();
            if (damageableObject != null)
            {
                // Calculate direction between avatar and monsters
                Vector3 parentPos = transform.parent.position;
                Vector2 direction = (Vector2)(enemy.gameObject.transform.position - parentPos).normalized;
                Vector2 knockback = direction * fakeHeight.knockbackForce;

                //collision.SendMessage("OnHit", swordDamage, knockback);
                damageableObject.OnHit(fakeHeight.seedDamage, knockback);
                slime.lastHitObject = "Newbie";

                float enemyHealth = enemy.transform.GetComponent<DamageableCharacter>().Health;
                if (enemyHealth <= 0)
                {
                    newbie.GetComponent<DamageableCharacter>().Aggro += 1f;
                    GameMaster.slimePiggle++;
                }
            }
            else
            {
                Debug.LogWarning("Collider does not implement IDmamageable");
            }
        }
    }
}
