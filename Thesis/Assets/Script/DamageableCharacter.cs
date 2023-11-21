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

    private Animator animator;
    private Collider2D colliderPhysics;
    private Rigidbody2D rb;

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

    void Start()
    {
        animator = gameObject.GetComponent<Animator>();

        rb = GetComponent<Rigidbody2D>();
        colliderPhysics = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnHit(float damage, Vector2 knockback)
    {
        Debug.Log("Hit");
        Health -= damage;
        animator.SetTrigger("Hit");

        rb.AddForce(knockback);
    }

    public void OnHit(float damage)
    {
        Debug.Log("Hit");
        Health -= damage;
        animator.SetTrigger("Hit");
    }

    public void Destroyself()
    {
        Destroy(gameObject);
    }
}
