using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{

    [Header("Attack")]
    [SerializeField] private float damage = 1f;
    [SerializeField] private float knockbackForce = 5f;
    [SerializeField] private float moveSpeed = 200f;

    private DetectionZone detectionZone;
    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 direction;

    private float horizontal;
    private float vertical;

    private Vector2 initPos;

    private void Awake()
    {
        detectionZone = transform.GetChild(0).GetComponent<DetectionZone>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        initPos = transform.position;
    }

    private void FixedUpdate()
    {
        if (detectionZone.detectObjects.Count > 0)
        {
            Collider2D detectedObject = detectionZone.detectObjects[0];
            if (detectedObject != null)
            {
                direction = (detectedObject.transform.position - transform.position).normalized;
                rb.AddForce(direction * moveSpeed * Time.deltaTime);
            }
        }

        horizontal = Mathf.Clamp(rb.velocity.x, -1, 1);
        vertical = Mathf.Clamp(rb.velocity.y, -1, 1);
        animator.SetFloat("Horizontal", horizontal);
        animator.SetFloat("Vertical", vertical);

        animator.SetFloat("Speed", rb.velocity.magnitude);
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
}
