using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class NewbieMovement : MonoBehaviour
{
    [Header("Panic")]
    public bool stopMoving;

    [Header("Avatar")]
    [SerializeField] private bool dialogueScene;

    [SerializeField] private GameObject piggleHealth;

    [Header("Communication Icon")]
    [SerializeField] private CommunictionIconTrigger communictionIconTrigger;

    private bool avatarFollow;

    //[Header("Enemies")]
    //public List<Collider2D> detectEnemies = new List<Collider2D>();

    private NavMeshAgent agent;


    private Vector2 wayPoint;
    private bool playerDetected;

    private DetectionZone detectionZone;
    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 direction;

    private float horizontal;
    private float vertical;

    private Vector3 initPos;

    // Line of sights for detecting avatar
    private bool hasLineOfSight;

    private bool moveToMonsterStart;
    private float distMin;
    private Transform target;
    private GameObject[] detectEnemies;

    private DialogueActivator dialogueActivator;
    private Transform player;

    private Image healthBar;
    private DamageableCharacter piggle;
    private void Awake()
    {
        detectionZone = transform.GetChild(0).GetComponent<DetectionZone>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        initPos = transform.position;

        player = GameObject.FindGameObjectWithTag("Player").transform;
        piggle = GetComponent<DamageableCharacter>();

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        stopMoving = true;
        target = transform;

        dialogueActivator = GetComponent<DialogueActivator>();
        if (!dialogueScene)
        {
            dialogueActivator.enabled = false;
            piggleHealth.SetActive(true);
            healthBar = piggleHealth.transform.GetChild(1).GetComponent<Image>();
        }
        else
        {
            dialogueActivator.enabled = true;
            piggleHealth.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        if(target == null)
        {
            distMin = 3000f;
            target = transform;
        }

        if (!dialogueScene) healthBar.fillAmount = piggle.Health / piggle.MaxHealth;

        if (!stopMoving)
        {
            agent.isStopped = false;
            

            if (dialogueScene)
            {
                agent.SetDestination(player.position);
                
            }
            else
            {
                if (!moveToMonsterStart)
                {
                    StartCoroutine("MoveToClosestMonster");
                }
                if ((rb.velocity.magnitude > 0.01f || !moveToMonsterStart))
                {
                    agent.SetDestination(target.position);
                }
                else if (moveToMonsterStart && rb.velocity.magnitude <= 0.01f)
                {
                    moveToMonsterStart = false;

                    distMin = 3000f;
                }

                
            }
        }
        else
        {
            agent.isStopped = true;
            //StopCoroutine("MoveToClosestMonster");
        }
        
       

        //horizontal = Mathf.Clamp(rb.velocity.x, -1, 1);
        //vertical = Mathf.Clamp(rb.velocity.y, -1, 1);
        //animator.SetFloat("Horizontal", horizontal);
        //animator.SetFloat("Vertical", vertical);

        //animator.SetFloat("Speed", rb.velocity.magnitude);

        // Line of sight
        
    }


    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    IDamageable damageable = collision.collider.GetComponent<IDamageable>();

    //    if (collision.collider.CompareTag("Monster"))
    //    {
    //        if (damageable != null)
    //        {
    //            Vector2 direction = (Vector2)(collision.gameObject.transform.position - transform.position).normalized;
    //            Vector2 knockback = direction * knockbackForce;

    //            //collision.SendMessage("OnHit", swordDamage, knockback);
    //            damageable.OnHit(damage, knockback);
    //        }
    //        else
    //        {
    //            Debug.LogWarning("Collider does not implement IDmamageable");
    //        }

    //    }
    //}


    private IEnumerator MoveToWayPoint()
    {
        // Back to init zone
        if (playerDetected)
        {
            yield return new WaitForSeconds(3f);
            playerDetected = false;
        }
    }

    private IEnumerator MoveToClosestMonster()
    {

        
        float dist;
        yield return new WaitForSeconds(0.5f);

        detectEnemies = GameObject.FindGameObjectsWithTag("Monster");
        foreach (GameObject enemy in detectEnemies)
        {
            dist = Vector2.Distance((Vector2)transform.position, (Vector2)enemy.transform.position);
            if(dist < distMin && enemy.GetComponent<Collider2D>() != null)
            {
                if (!GameMaster.commTriggered)
                {
                    distMin = dist;
                    target = enemy.transform;
                }
                else
                {
                    if (!communictionIconTrigger.signDropped)
                    {
                        distMin = dist;
                        target = enemy.transform;
                    }
                    else
                    {
                        target = communictionIconTrigger.signPos;
                    }
                }
                
                
            }
        }

        moveToMonsterStart = true;
        yield return new WaitForSeconds(2f);
    }

}
