using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NewbieMovement : MonoBehaviour
{
    [Header("Attack")]
    [SerializeField] private float damage = 1f;
    [SerializeField] private float knockbackForce = 5f;
    [SerializeField] private float moveSpeed = 200f;

    [Header("Raycast")]
    [SerializeField] private LayerMask layerMaskNewbie;

    [Header("Panic")]
    public bool stopMoving;

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
    private Vector2 targetPos;
    private GameObject[] detectEnemies;


    private void Awake()
    {
        detectionZone = transform.GetChild(0).GetComponent<DetectionZone>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        initPos = transform.position;


        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        stopMoving = true;
    }

    private void FixedUpdate()
    {
        
        if (!stopMoving)
        {
            if (!moveToMonsterStart)
            {
                StartCoroutine(MoveToClosestMonster());
            }
            if ((rb.velocity.magnitude > 0.01f || !moveToMonsterStart))
            {
                agent.SetDestination(targetPos);
            }
            else if (moveToMonsterStart && rb.velocity.magnitude <= 0.01f)
            {
                moveToMonsterStart = false;

                distMin = 3000f;
            }
        }
        else
        {
            StopCoroutine(MoveToClosestMonster());
        }
        
       

        //horizontal = Mathf.Clamp(rb.velocity.x, -1, 1);
        //vertical = Mathf.Clamp(rb.velocity.y, -1, 1);
        //animator.SetFloat("Horizontal", horizontal);
        //animator.SetFloat("Vertical", vertical);

        //animator.SetFloat("Speed", rb.velocity.magnitude);

        // Line of sight
        
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        IDamageable damageable = collision.collider.GetComponent<IDamageable>();

        if (collision.collider.CompareTag("Monster"))
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

        yield return new WaitForSeconds(2f);

        detectEnemies = GameObject.FindGameObjectsWithTag("Monster");
        foreach (GameObject enemy in detectEnemies)
        {
            dist = Vector2.Distance((Vector2)transform.position, (Vector2)enemy.transform.position);
            if(dist < distMin)
            {
                targetPos = (Vector2)enemy.transform.position;
            }
        }

        moveToMonsterStart = true;
        yield return new WaitForSeconds(2f);
    }

}
