using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public bool jumpEnabled;

    [Header("Player Movement")]
    public float PlayerSpeed = 5f;
    [SerializeField] float maxSpeed = 5f;
    public Vector2 movement;
    public bool stopMovementInput;
    public Vector2 moveDir;
    

    [Header("Dialogue")]
    [SerializeField] private DialogueUI dialogueUI;

    [Header("Hit Box")]
    [SerializeField] private GameObject[] hitBoxes;


    [Header("Anim Intervene")]
    public bool animControll;

    public DialogueUI DialogueUI => dialogueUI;
    
    public PlayerInteractable Interactable { get; set; }
    
    private Rigidbody2D rb;

    private Animator animator;
    
    private Vector2 lastMoveDir;

    
    private float idleFriction = 0.6f;
    private bool canMove = true;


    private Collider2D[] swordCollider = new Collider2D[4];

    private float attackInterval = 1.2f;
    private bool attackEnabled = true;
    private float attackTimeElapsed;


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


        if (canMove && !stopMovementInput && !animControll)
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
        }else if (animControll) {
            movement.x = Mathf.Clamp(rb.velocity.x, -1, 1);
            movement.y = Mathf.Clamp(rb.velocity.y, -1, 1);
        }
        

        if (canMove && (movement.x == 0 && movement.y == 0) && (moveDir.x != 0 || moveDir.y != 0))
        {
            lastMoveDir = moveDir;
        }

        if (!animControll) moveDir = new Vector2(movement.x, movement.y).normalized;

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

        if (!attackEnabled)
        {
            attackTimeElapsed += Time.deltaTime;
            if (attackTimeElapsed > attackInterval)
            {
                attackEnabled = true;
            }
        }
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            rb.AddForce(moveDir * PlayerSpeed * Time.deltaTime);
            if (rb.velocity.magnitude > maxSpeed)
            {
                float limitedSpeed = Mathf.Lerp(rb.velocity.magnitude, maxSpeed, idleFriction);
                rb.velocity = rb.velocity.normalized * limitedSpeed;
            }

        }
        else
        {
            rb.velocity = Vector2.Lerp(rb.velocity, Vector2.zero, idleFriction);
        }
       
    }


    private void OnFire()
    {
        if (attackEnabled)
        {
            animator.SetTrigger("swordAttack");
            attackEnabled = false;
            attackTimeElapsed = 0f;
        }

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
