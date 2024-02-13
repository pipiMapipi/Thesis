using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BoundDetect : MonoBehaviour
{
    [Header("Bound Check")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float destroyTime = 1f;
    [SerializeField] private float distMax;
    [SerializeField] private float extendedDestroyTime = 2f;

    private Vector2 direction;
    private GameObject player;
    private Rigidbody2D rb;
    private bool boundNow;
    private bool boundChecking = true;
    private float dist;


    void Start()
    {
        
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

        direction = (player.transform.position - transform.position).normalized;
        
        dist = Vector3.Distance(this.transform.position, transform.parent.gameObject.transform.position);
        if(dist <= distMax)
        {
            transform.Translate(direction * moveSpeed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector2.zero);
        }

        if (boundChecking)
        {
            StartCoroutine(CheckPlayerInReach());
        }

         
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            boundNow = true;
        }
    }

    private IEnumerator CheckPlayerInReach()
    {
        boundChecking = false;
        yield return new WaitForSeconds(destroyTime);
        boundChecking = true;
        if (!boundNow)
        {
            Destroy(gameObject);
        }
        else
        {
            FreezePlayer();
            yield return new WaitForSeconds(extendedDestroyTime);
            Destroy(gameObject);
            DefreezePlayer();
        }
    }

    private void FreezePlayer()
    {
        player.transform.position = this.transform.position;
        player.GetComponent<PlayerMovement>().enabled = false;
        player.GetComponent<PlayerInput>().enabled = false;
    }

    private void DefreezePlayer()
    {
        player.GetComponent<PlayerMovement>().enabled = true;
        player.GetComponent<PlayerInput>().enabled = true;
    }
}
