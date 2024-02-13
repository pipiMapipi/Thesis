using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{

    [Header("Attack")]
    [SerializeField] private float damage = 1f;
    [SerializeField] private float knockbackForce = 5f;
    [SerializeField] private float moveSpeed = 200f;

    [Header("Raycast")]
    [SerializeField] private LayerMask layerMask;

    [Header("Wander")]
    [SerializeField] private float moveRange;
    [SerializeField] private float maxDist;
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

    private void Awake()
    {
        detectionZone = transform.GetChild(0).GetComponent<DetectionZone>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        initPos = transform.position;

        player = GameObject.FindGameObjectWithTag("Player");
        
        SetWanderPos();
        
    }

    private void FixedUpdate()
    {
        if (detectionZone.detectObjects.Count > 0)
        {
            Collider2D detectedObject = detectionZone.detectObjects[0];
            if (detectedObject != null && hasLineOfSight)
            {
                direction = (detectedObject.transform.position - transform.position).normalized;
                rb.AddForce(direction * moveSpeed * Time.deltaTime);
                playerDetected = true;
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

        // Line of sight
        RaycastHit2D ray = Physics2D.Raycast(transform.position, player.transform.position - transform.position, layerMask);
        if(ray.collider != null)
        {
            hasLineOfSight = ray.collider.CompareTag("Player");
            if (hasLineOfSight)
            {
                Debug.DrawRay(transform.position, player.transform.position - transform.position, Color.green);
            }else
            {
                Debug.DrawRay(transform.position, player.transform.position - transform.position, Color.red);
            }
        }
    }


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
}
