using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpNow : MonoBehaviour
{
    [SerializeField] private bool isHorizontalJump;
    [SerializeField] private Transform newPos;
    

    [SerializeField] private float groundDispenseVel;
    [SerializeField] private float verticalDispenseVel;

    [Header("Player Sprites")]
    [SerializeField] private Animator playerJump;
    private SpriteRenderer playerDefault;
    

    private Transform player;
    private PlayerMovement playerMovement;
    private Vector2 initPos;
    private bool jumpEnabled;

    private Vector2 direction;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerMovement = player.GetComponent<PlayerMovement>();

        playerDefault = player.GetComponent<SpriteRenderer>();
        playerJump.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        

        

        if (Input.GetKeyDown(KeyCode.Space) && jumpEnabled)
        {
            jumpEnabled = false;
            initPos = transform.position;
            Debug.Log(initPos.x + ";" + newPos.position.x);
            StartCoroutine(Jump());
        }
    }

    private IEnumerator Jump()
    {
        playerDefault.enabled = false;
        playerJump.enabled = true;

        if (isHorizontalJump)
        {
            if (newPos.position.x > initPos.x)
            {
                playerJump.SetBool("jumpLeft", false);
                direction = transform.right;
            }
            else
            {
                playerJump.SetBool("jumpLeft", true);
                direction = -transform.right;
            }
        }
        else
        {
            if (newPos.position.y > initPos.y)
            {
                direction = transform.up;
            }
            else
            {
                direction = -transform.up;
            }
        }


        yield return new WaitForSeconds(1f);
        playerJump.SetBool("jumpNow", true);

        
        player.GetComponent<FakeHeight>().Initialize(direction * groundDispenseVel, verticalDispenseVel);
        

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            jumpEnabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            jumpEnabled = false;
        }
    }
}
