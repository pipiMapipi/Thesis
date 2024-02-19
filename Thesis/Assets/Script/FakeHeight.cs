using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FakeHeight : MonoBehaviour
{
    public UnityEvent onGroundHitEvent;

    [SerializeField] private Transform transObject;
    [SerializeField] private Transform transMain;
    [SerializeField] private Transform transShadow;
    [SerializeField] private Transform damageArea;

    [SerializeField] private float gravity = -10f;
    [SerializeField] private Vector2 groundVel;
    [SerializeField] private float verticalVel;

    [SerializeField] private bool isGrounded;

    [Header("Trap")]
    [SerializeField] private float maxRadius;
    [SerializeField] private float expandTime;
    [SerializeField] private float explodeTime;
    [SerializeField] private Animator trapAnim;
    

    private float lastInitialVerticalVel;
    private float explosionStart;
    private SpriteRenderer damageSprite;
    private bool hasLanded;
    private int expansionStep;
    private float elapsedTime;
    private bool startExplosion;


    void Start()
    {
        damageSprite = damageArea.GetComponent<SpriteRenderer>();
        expansionStep = 0;
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePosition();
        CheckGroundHit();

        if (hasLanded)
        {  
            TrapAreaIndicator();            
        }
        if (startExplosion)
        {
            ExplosionAttack();
        }
        
    }

    void UpdatePosition()
    {
        if (!isGrounded)
        {
            verticalVel += gravity * Time.deltaTime;
            transMain.position += new Vector3(0, verticalVel, 0) * Time.deltaTime;
        }
        
        transObject.position += (Vector3)groundVel * Time.deltaTime;
    }

    public void Initialize(Vector2 groundVel, float verticalVel)
    {
        isGrounded = false;
        this.groundVel = groundVel;
        this.verticalVel = verticalVel;
        lastInitialVerticalVel = verticalVel;
    }

    void CheckGroundHit()
    {
        if(transMain.position.y < transObject.position.y && !isGrounded)
        {
            transMain.position = transObject.position;
            isGrounded = true;
            GroundHit();
        }
    }

    void GroundHit()
    {
        onGroundHitEvent.Invoke();
    }

    public void Land()
    {
        groundVel = Vector2.zero;
    }

    public void Bounce(float divisionFactor)
    {
        Initialize(groundVel, lastInitialVerticalVel / divisionFactor);
    }

    public void SlowDown(float divisionFactor)
    {
        groundVel = groundVel / divisionFactor;
        if(groundVel == Vector2.zero)
        {
            hasLanded = true;
        }
    }

    private void TrapAreaIndicator()
    {

        if(expansionStep < 3)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime > expandTime / 3f)
            {
                expansionStep++;
                elapsedTime = 0;
                float newRadius = maxRadius * (expansionStep / 3f);
                damageArea.localScale = new Vector3(newRadius, newRadius, 1);
            }

        }     
        else
        {
            damageArea.localScale = new Vector3(maxRadius, maxRadius, 1);
            startExplosion = true;
        }
        
    }

    private void ExplosionAttack()
    {
        explosionStart += Time.deltaTime;
        
        if(explosionStart > explodeTime)
        {
            Destroy(gameObject);
        }
        else
        {
            trapAnim.SetTrigger("countDown");
        }

    }

}
