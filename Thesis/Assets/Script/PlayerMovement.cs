using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public bool jumpEnabled;
    public float jumpForce;

    [Header("Player Movement")]
    [SerializeField] float PlayerSpeed = 5f;
    [SerializeField] float maxSpeed = 5f;

    [Header("Dialogue")]
    [SerializeField] private DialogueUI dialogueUI;

    [Header("Hit Box")]
    [SerializeField] private GameObject[] hitBoxes;

    [Header("Newbie")]
    [SerializeField] Newbie newbie;
    [SerializeField] float speedOffset = -100f;
    [SerializeField] float maxSpeedOffset = -1f;

    public DialogueUI DialogueUI => dialogueUI;
    public PlayerInteractable Interactable { get; set; }
    
    private Rigidbody2D rb;

    private Animator animator;
    private Vector2 moveDir;
    private Vector2 lastMoveDir;

    private Vector2 movement;
    private float idleFriction = 0.6f;

    private bool canMove = true;

    private Collider2D[] swordCollider = new Collider2D[4];

    
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        animator = GetComponent<Animator>();

        for(int i = 0; i < hitBoxes.Length; i++)
        {
            swordCollider[i] = hitBoxes[i].GetComponent<Collider2D>();
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        if (dialogueUI != null)
        {
            if (dialogueUI.IsOpen) return; // stop moving when talking
        }

        if (canMove)
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
        }
        

        if (canMove && (movement.x == 0 && movement.y == 0) && (moveDir.x != 0 || moveDir.y != 0))
        {
            lastMoveDir = moveDir;
        }

        moveDir = new Vector2(movement.x, movement.y).normalized;

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);
        animator.SetFloat("LMHorizontal", lastMoveDir.x);
        animator.SetFloat("LMVertical", lastMoveDir.y);

        

        if (Input.GetKeyDown(KeyCode.Space))
        {
            
            //Jump();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Interactable != null)
            {
                Interactable.Interact(this);
            }
        }
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            if (newbie.isCarrying) {
                rb.AddForce(moveDir * (PlayerSpeed + speedOffset) * Time.deltaTime);
                if (rb.velocity.magnitude > maxSpeed)
                {
                    float limitedSpeed = Mathf.Lerp(rb.velocity.magnitude, maxSpeed + maxSpeedOffset, idleFriction);
                    rb.velocity = rb.velocity.normalized * limitedSpeed;
                }
            }
            else
            {
                rb.AddForce(moveDir * PlayerSpeed * Time.deltaTime);
                if (rb.velocity.magnitude > maxSpeed)
                {
                    float limitedSpeed = Mathf.Lerp(rb.velocity.magnitude, maxSpeed, idleFriction);
                    rb.velocity = rb.velocity.normalized * limitedSpeed;
                }
            }
            
        }
        else
        {
            rb.velocity = Vector2.Lerp(rb.velocity, Vector2.zero, idleFriction);
        }
       
    }

    private void Jump()
    {
        rb.AddForce(Vector2.right * jumpForce, ForceMode2D.Impulse);
        Debug.Log("jump");
    }

    private void OnFire()
    {
        animator.SetTrigger("swordAttack");
    }

    private void LockMovement()
    {
        canMove = false;
    }

    private void UnLockMovement()
    {
        canMove = true;
    }
}
