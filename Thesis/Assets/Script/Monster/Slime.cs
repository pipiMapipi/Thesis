using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float _health = 3f;

    private Animator animator;

    // Property
    public float Health
    {
        set
        {
            _health = value;

            if (_health <= 0)
            {
                animator.SetTrigger("Death");
                //Destroy(gameObject);
            }
        }

        get
        {
            return _health;
        }
    }

    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnHit(float damage)
    {
        Debug.Log("Hit");
        Health -= damage;
        animator.SetTrigger("Hit");
    }
}
