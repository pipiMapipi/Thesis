using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SwordHitBox : MonoBehaviour
{

    [SerializeField] private float swordDamage = 1f;
    [SerializeField] private float knockbackForce = 20f;

    private Collider2D swordCollider;
    private Transform player;

    private PiggleCommunica piggleCommunica;
   
    void Start()
    {
        swordCollider = gameObject.GetComponent<Collider2D>();
        player = transform.parent.transform;

        if (SceneManager.GetActiveScene().name == "Combat")  piggleCommunica = GameObject.FindGameObjectWithTag("PiggleSign").GetComponent<PiggleCommunica>();
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
            Slime slime = collision.GetComponent<Slime>();
            if (damageableObject != null)
            {
                // Calculate direction between avatar and monsters
                Vector3 parentPos = transform.parent.position;
                Vector2 direction = (Vector2)(collision.gameObject.transform.position - parentPos).normalized;
                Vector2 knockback = direction * knockbackForce;

                //collision.SendMessage("OnHit", swordDamage, knockback);
                damageableObject.OnHit(knockback);
                slime.lastHitObject = "Player";

                float enemyHealth = collision.transform.GetComponent<DamageableCharacter>().Health;
                if(enemyHealth == 0)
                {
                    player.GetComponent<DamageableCharacter>().Aggro += 1f;
                }
            }
            else
            {
                Debug.LogWarning("Collider does not implement IDmamageable");
            }
            
        }

        if (collision.CompareTag("Flower"))
        {
            // check if there is IDamageable in the script of the gameobject
            IDamageable damageableObject = collision.GetComponent<IDamageable>();
            if (damageableObject != null)
            {
                Animator flowerAnim = collision.transform.parent.GetChild(1).GetComponent<Animator>();
                Image flowerHealthBar = collision.transform.parent.GetChild(1).GetChild(0).GetChild(0).GetComponent<Image>();
 
                damageableObject.OnHit(swordDamage);


                float flowerHealth = collision.transform.GetComponent<DamageableCharacter>().Health;
                float flowerMaxHealth = collision.transform.GetComponent<DamageableCharacter>().MaxHealth;

                StartCoroutine(FlowerGetHit(flowerAnim, flowerHealthBar, flowerHealth, flowerMaxHealth));

                if (flowerHealth == 0)
                {
                    GameObject flowerParent = collision.transform.parent.gameObject;
                    StartCoroutine(DestoryFlower(flowerAnim, flowerParent));
                    
                }
            }
            else
            {
                Debug.LogWarning("Collider does not implement IDmamageable");
            }

        }


    }

    private IEnumerator FlowerGetHit(Animator flower, Image healthBar, float health, float maxHealth)
    {
        healthBar.fillAmount = health / maxHealth;
        flower.SetBool("GetHit", true);
        yield return new WaitForSeconds(0.5f);
        flower.SetBool("GetHit", false);
    }

    private IEnumerator DestoryFlower(Animator flower, GameObject flowerParent)
    {
        flower.SetBool("FadeOut", true);
        piggleCommunica.taskSolved = true;
        piggleCommunica.needClearFlower = false;
        yield return new WaitForSeconds(2f);      
        Destroy(flowerParent);
    }
}

