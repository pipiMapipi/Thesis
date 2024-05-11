using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamAttack : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private LineRenderer linerenderer;
    [SerializeField] private Transform firePoint;

    [SerializeField] private LayerMask layerMask;

    [Header("VFX")]
    [SerializeField] private GameObject startVFX;
    [SerializeField] private GameObject endVFX;

    [Header("Attack")]
    [SerializeField] private float damage;
    [SerializeField] private float knockbackForce;
    [SerializeField] private float attackInterval = 2f;

    private ZoneAttack zoneAttack;
    private Quaternion rotation;
    private List<ParticleSystem> particles = new List<ParticleSystem>();
    private bool hasInitVFX;

    private bool hasRunOnEnable;
    private float attackIntervalTime;

    private Transform newbie;

    void OnEnable()
    {        
        if(!hasRunOnEnable)
        {
            startVFX.SetActive(false);
            endVFX.SetActive(false);
            AddToList();
            DisableBeam();
            zoneAttack = transform.parent.GetComponent<ZoneAttack>();
            attackIntervalTime = attackInterval;

            hasRunOnEnable = true;
        }

        newbie = transform.parent.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(zoneAttack.enemyList.Count != 0)
        {
            if (!hasInitVFX)
            {
                hasInitVFX = true;
                startVFX.SetActive(true);
                endVFX.SetActive(true);
            }         

            EnableBeam();
            InitBeamAttack();
            
        }
        else
        {
            DisableBeam();
        }
        

        //Debug
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    EnableBeam();
        //}

        //if (Input.GetKey(KeyCode.Space))
        //{
        //    UpdateBeam();
        //}

        //if (Input.GetKeyUp(KeyCode.Space))
        //{
        //    DisableBeam();
        //}

    }

    private void EnableBeam()
    {
        linerenderer.enabled = true;


        for (int i = 0; i < particles.Count; i++)
        {
            if (!particles[i].isPlaying)
            {
                particles[i].Play();
            }
            
        }      
    }

    private void UpdateBeam(Vector2 enemyPos)
    {
        //var mousePos = (Vector2)cam.ScreenToWorldPoint(Input.mousePosition);
        linerenderer.SetPosition(0, (Vector2)firePoint.position);
        startVFX.transform.position = (Vector2)firePoint.position;

        linerenderer.SetPosition(1, enemyPos);

        Vector2 direction = enemyPos - (Vector2)transform.position;
        RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position, direction.normalized, direction.magnitude, layerMask);
        if (hit)
        {
            linerenderer.SetPosition(1, hit.point);

            IDamageable damageable = hit.collider.GetComponent<IDamageable>();

            if (hit.collider.CompareTag("Monster"))
            {
                if (damageable != null)
                {
                    Vector2 direct = (Vector2)(hit.collider.gameObject.transform.position - transform.position).normalized;
                    Vector2 knockback = direct * knockbackForce;



                    attackIntervalTime += Time.deltaTime;
                    //collision.SendMessage("OnHit", swordDamage, knockback);
                    if(attackIntervalTime > attackInterval)
                    {
                        damageable.OnHit(damage, knockback);
                        attackIntervalTime = 0;
                    }


                    float enemyHealth = hit.collider.transform.GetComponent<DamageableCharacter>().Health;
                    if (enemyHealth == 0)
                    {
                        newbie.GetComponent<DamageableCharacter>().Aggro += 1f;
                    }

                }
                else
                {
                    Debug.LogWarning("Collider does not implement IDmamageable");
                }

            }
        }
        endVFX.transform.position = linerenderer.GetPosition(1);
    }

    private void DisableBeam()
    {
        linerenderer.enabled = false;

        for (int i = 0; i < particles.Count; i++)
        {
            if (particles[i].isPlaying)
            {
                particles[i].Stop();
            }
            
        }
        
    }

    private void RotateToTarget(Vector2 enemyPos)
    {
        //Vector2 direction = cam.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        Vector2 direction = enemyPos - (Vector2)transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rotation.eulerAngles = new Vector3(0, 0, angle);
        transform.rotation = rotation;
    }

    private void InitBeamAttack()
    {
        Vector2 enemyPos = (Vector2)zoneAttack.enemyList[0].transform.position;
        RotateToTarget(enemyPos);
        UpdateBeam(enemyPos);
        
    }

    private void AddToList()
    {
        for(int i = 0; i < startVFX.transform.childCount; i++)
        {
            var ps = startVFX.transform.GetChild(i).GetComponent<ParticleSystem>();
            if(ps != null )
            {
                particles.Add(ps);
            }
        }

        

        for (int i = 0; i < endVFX.transform.childCount; i++)
        {
            var ps = endVFX.transform.GetChild(i).GetComponent<ParticleSystem>();
            if (ps != null)
            {
                particles.Add(ps);
            }
        }
    }
}
