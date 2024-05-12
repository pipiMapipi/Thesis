using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Slime : MonoBehaviour
{

    [Header("Attack")]
    [SerializeField] private float damage = 1f;
    [SerializeField] private float knockbackForce = 5f;
    [SerializeField] private float moveSpeed = 200f;

    [Header("Raycast")]
    [SerializeField] private LayerMask layerMaskSlime;

    [Header("Wander")]
    [SerializeField] private float moveRange;
    [SerializeField] private float maxDist;

    [Header("Potion")]
    public float potionProb;

    public string lastHitObject;
    public float speedNow;

    private Transform[] targets = new Transform[2];
    private NavMeshAgent agent;
    private Transform target;
    private string tag;

    
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
    private GameObject player;
    private GameObject newbie;

    
    private void Awake()
    {
        detectionZone = transform.GetChild(0).GetComponent<DetectionZone>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        initPos = transform.position;

        player = GameObject.FindGameObjectWithTag("Player");
        newbie = GameObject.FindGameObjectWithTag("Newbie");
        
        SetWanderPos();

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        targets[0] = player.transform;
        targets[1] = newbie.transform;

        potionProb = Random.Range(0f, 1f);
    }

    private void FixedUpdate()
    {
        if (GetComponent<DamageableCharacter>().Health <= 0)
        {
            agent.SetDestination(transform.position);
        }

        if (lastHitObject == "")
        {
            CheckAggro();
        }
        else
        {
            if (lastHitObject == "Player")
            {
                target = targets[0];
                tag = "Player";
            }
            else
            {
                target = targets[1];
                tag = "Newbie";
            }
        }
        

        if (detectionZone.detectObjects.Count > 0)
        {
            for (int i = 0; i < detectionZone.detectObjects.Count; i++)
            {
                if (detectionZone.detectObjects[i].CompareTag(tag))
                {
                    Collider2D detectedObject = detectionZone.detectObjects[i];

                    if (detectedObject != null && hasLineOfSight && GetComponent<DamageableCharacter>().Health > 0)
                    {
                        //direction = (detectedObject.transform.position - transform.position).normalized;
                        //rb.AddForce(direction * moveSpeed * Time.deltaTime);

                        agent.SetDestination(target.position);
                        playerDetected = true;
                    } 
                }

                
            }
             
            
        } else
        {
            if (!playerDetected)
            {
                direction = (wayPoint - (Vector2)transform.position).normalized;
                rb.AddForce(direction * moveSpeed * Time.deltaTime);
            }
            StartCoroutine(MoveToWayPoint());
            
            //StartCoroutine(BackToInitPos(initPos));
        }

        horizontal = Mathf.Clamp(rb.velocity.x, -1, 1);
        vertical = Mathf.Clamp(rb.velocity.y, -1, 1);
        animator.SetFloat("Horizontal", horizontal);
        animator.SetFloat("Vertical", vertical);

        animator.SetFloat("Speed", rb.velocity.magnitude);
        speedNow = rb.velocity.magnitude;
        // Line of sight
        RaycastHit2D ray = Physics2D.Raycast(transform.position, (Vector2)(GameObject.FindGameObjectWithTag(tag).transform.position - transform.position).normalized, (GameObject.FindGameObjectWithTag(tag).transform.position - transform.position).magnitude, layerMaskSlime);
        if(ray.collider != null)
        {

            hasLineOfSight = ray.collider.CompareTag(tag);
            if (hasLineOfSight)
            {
                Debug.DrawRay(transform.position, GameObject.FindGameObjectWithTag(tag).transform.position - transform.position, Color.green);
            }else
            {
                Debug.DrawRay(transform.position, GameObject.FindGameObjectWithTag(tag).transform.position - transform.position, Color.red);
            }
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        IDamageable damageable = collision.collider.GetComponent<IDamageable>();

        if (collision.collider.CompareTag("Player") || collision.collider.CompareTag("Newbie"))
        {
            if (damageable != null)
            {
                Vector2 direction = (Vector2)(collision.gameObject.transform.position - transform.position).normalized;
                Vector2 knockback = direction * knockbackForce;

                //collision.SendMessage("OnHit", swordDamage, knockback);
                damageable.OnHit(damage, knockback);
            }
            else
            {
                Debug.LogWarning("Collider does not implement IDmamageable");
            }

        }
    }

    private IEnumerator BackToInitPos(Vector3 initPos)
    {
        yield return new WaitForSeconds(3f);

        direction = (initPos - transform.position).normalized;
        rb.AddForce(direction * moveSpeed * Time.deltaTime);
    }

    private void SetWanderPos()
    {
        wayPoint = new Vector2(Random.Range(-maxDist, maxDist) + initPos.x, Random.Range(-maxDist, maxDist) + initPos.y);
        

    }

    private IEnumerator MoveToWayPoint()
    {
        // Back to init zone
        if (playerDetected)
        {
            yield return new WaitForSeconds(3f);
            playerDetected = false;
        }
        else
        {
            if (Vector2.Distance(transform.position, wayPoint) < moveRange)
            {
                yield return new WaitForSeconds(2f);
                SetWanderPos();
                
            }
        }
        

    }

    private void CheckAggro()
    {
        float[] aggro = new float[2];
        for(int i = 0; i < targets.Length; i++)
        {
            aggro[i] = targets[i].GetComponent<DamageableCharacter>().Aggro;
        }
        
        if(aggro[0] >= aggro[1])
        {
            target = targets[0];
            tag = "Player";
        }
        else
        {
            target = targets[1];
            tag = "Newbie";
        }
    }
}
