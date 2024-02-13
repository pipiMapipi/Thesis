using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WithShield : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private TrailRenderer tr;

    [Header("Stage 1")]
    [SerializeField] private float dashSpeed1 = 10f;
    [SerializeField] private float dashDuration1 = 0.1f;
    [SerializeField] private float dashInterval1 = 2f;
    [SerializeField] private GameObject boundPrefab;


    [Header("Stage 2")]
    [SerializeField] private float dashSpeed2 = 20f;
    [SerializeField] private float dashDuration2 = 0.2f;
    [SerializeField] private float dashInterval2 = 2f;

    private int stage = 1;
    private Vector3 direction;
    private GameObject player;
    private Rigidbody2D rb;
    private bool isDashing;
    private bool canDash = true;

    private bool canBound = true;
 

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Do nothing when dashing
        if (isDashing)
        {
            return;
        }

        if (transform.GetChild(0).gameObject.activeSelf)
        {
            stage = 1;
        }
        else
        {
            stage = 2;
        }

        // Stage 1: Short-range Dash & Bound



        // Stage 2: Long-range Dash
        if(Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            if(stage == 1)
            {
                StartCoroutine(Dash(dashSpeed1, dashDuration1, dashInterval1));
                if (isDashing && canBound)
                {
                    StartCoroutine(Bound(dashDuration1));
                }
            }
            if(stage == 2)
            {
                StartCoroutine(Dash(dashSpeed2, dashDuration2, dashInterval2));
            }
            
        }
    }


    private IEnumerator Dash(float dashSpeed, float dashDuration, float dashInterval)
    {
        canDash = false;
        isDashing = true;
        direction = (player.transform.position - transform.position).normalized;
        rb.velocity = direction * dashSpeed;
        tr.emitting = true;
        yield return new WaitForSeconds(dashDuration);

        

        tr.emitting = false;
        isDashing = false;
        yield return new WaitForSeconds(dashInterval);
        canDash = true;
        
    }

    private IEnumerator Bound(float dashDuration)
    {
        canBound = false;
        yield return new WaitForSeconds(dashDuration);
        Instantiate(boundPrefab, this.transform);
        yield return null;
        canBound = true;

    }
}
