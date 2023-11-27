using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageableCharacter : MonoBehaviour, IDamageable
{
    [Header("Health")]
    [SerializeField] private float _health = 3f;

    [Header("Targetable")]
    [SerializeField] private bool _targetable = true;

    [Header("Disable Physics")]
    [SerializeField] private bool disableSimulation = false;

    [Header("Invincibility")]
    [SerializeField] private float invincibilityTime = 0.8f;
    [SerializeField] private bool invincibilityEnabled = true;
    [SerializeField] private bool _invincible;

    private Animator animator;
    private Collider2D colliderPhysics;
    private Rigidbody2D rb;

    private float invincibleTimeElapsed = 0f;

    // Property
    public float Health
    {
        set
        {
            _health = value;

            if (_health <= 0)
            {
                animator.SetTrigger("Death");
                Targetable = false;
                //Destroy(gameObject);
            }
        }

        get
        {
            return _health;
        }
    }

    public bool Targetable
    {
        set
        {
            _targetable = value;

            if (disableSimulation)
            {
                rb.simulated = value;
            }

            colliderPhysics.enabled = value;
        }

        get
        {
            return _targetable;
        }
    }

    public bool Invincible
    {
        set
        {
            _invincible = value;
            if(_invincible)
            {
                invincibleTimeElapsed = 0f;
            }
        }

        get
        {
            return _invincible;
        }
    }

    void Start()
    {
        animator = gameObject.GetComponent<Animator>();

        rb = GetComponent<Rigidbody2D>();
        colliderPhysics = GetComponent<Collider2D>();
    }

    private void FixedUpdate()
    {
        if (Invincible)
        {
            invincibleTimeElapsed += Time.deltaTime;
            if(invincibleTimeElapsed > invincibilityTime)
            {
                Invincible = false;
            }
        }
    }

    public void OnHit(float damage, Vector2 knockback)
    {
        if (!Invincible)
        {
            Health -= damage;
            animator.SetTrigger("Hit");

            rb.AddForce(knockback, ForceMode2D.Impulse);

            if (invincibilityEnabled)
            {
                Invincible = true;
            }
        }
    }

    public void OnHit(float damage)
    {
        if (!Invincible)
        {
            Health -= damage;
            animator.SetTrigger("Hit");

            if (invincibilityEnabled)
            {
                Invincible = true;
            }
        }
        
    }

    public void Destroyself()
    {
        Destroy(gameObject);
    }
}
