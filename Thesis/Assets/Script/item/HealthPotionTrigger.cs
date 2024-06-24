using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotionTrigger : MonoBehaviour
{
    public GameObject entity;
    public GameObject potion;

    [SerializeField] private bool randomDrop;

    private Slime slime;
    private float triggerAmount = 0.6f;
    [SerializeField] private bool canDrop;
    private Vector3 velocity = Vector3.zero;
    private float smoothTime = 0.1f;
    private Transform target;
    private Animator anim;
    private bool getNow;
    private float healthAmount = 30f;
    private bool posInfoStored;
    private Vector2 posOffset = new Vector2(0, 1.2f);
    void Start()
    {
        if (randomDrop)
        {
            slime = entity.GetComponent<Slime>();
            if (slime.potionProb >= triggerAmount) canDrop = true;
            target = GameObject.FindGameObjectWithTag("Newbie").transform;
        }
        else
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }
        anim = potion.transform.GetChild(0).GetComponent<Animator>();
        anim.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(entity != null)
        {
            if (entity.GetComponent<DamageableCharacter>().Health <= 0 && !posInfoStored)
            {
                posInfoStored = true;
                

                if ((!randomDrop || (randomDrop && canDrop)))
                {
                    anim.enabled = true;
                    if (target.GetComponent<DamageableCharacter>().cureAnim.activeSelf)
                    {
                        target.GetComponent<DamageableCharacter>().cureAnim.SetActive(false);
                    }
                }
            }
            potion.transform.position = (Vector2) entity.transform.position + posOffset;
        }



        if (anim.enabled)
        {
            if (getNow)
            {
                potion.transform.position = Vector3.SmoothDamp(potion.transform.position, (Vector2)target.position, ref velocity, smoothTime);
                float dist = Vector2.Distance((Vector2)potion.transform.position, (Vector2)target.position);
                if (dist < 0.2f)
                {
                    anim.SetTrigger("Cure");
                    StartCoroutine(AddToEntityHealth());
                }
            }
            else
            {
                StartCoroutine(IncreaseHealth());
            }
        }
    }

    IEnumerator IncreaseHealth()
    {
        yield return new WaitForSeconds(2f);
        getNow = true;   
    }

    IEnumerator AddToEntityHealth()
    {
        yield return new WaitForSeconds(0.5f);

        float health = target.GetComponent<DamageableCharacter>().Health;
        float maxHealth = target.GetComponent<DamageableCharacter>().MaxHealth;

        if (health + healthAmount > maxHealth)
        {
            target.GetComponent<DamageableCharacter>().Health = maxHealth;
        }
        else
        {
            target.GetComponent<DamageableCharacter>().Health += healthAmount;
        }

        if(target.GetComponent<DamageableCharacter>().cureAnim != null && entity == null)
        {
            target.GetComponent<DamageableCharacter>().cureAnim.SetActive(true);
        }

        Destroy(this);
    }
}
