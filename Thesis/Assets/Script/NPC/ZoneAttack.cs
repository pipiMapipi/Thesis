using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ZoneAttack : MonoBehaviour
{

    //[Header("Zone")]
    //[SerializeField] private bool enterComfortZone;
    //[SerializeField] private bool enterPanicZone;
    //[SerializeField] private bool enterShutdownZone;

    public List<Collider2D> enemyList = new List<Collider2D>();

    [Header("Zone Chnage")]
    [SerializeField] private float BEDZ; //Bearable enemy density within the zone
    [SerializeField] private float CEDZ; //Current enemy density within the zone
    [SerializeField] private float maxHP;
    [SerializeField] private float currentHP;

    private GameObject comfortZoneAttack;
    private GameObject panicZoneAttack;
    private GameObject shutdownZoneAttack;

    private TrapAttack trapAttack;

    private Collider2D zoneCollider;

    private NewbieMovement newbieMovement;

    private float CPborder = 0.5f;
    private float PSborder = 0.7f;
    private float HP;

    private PiggleCommunica piggleCommunica;

    void Start()
    {
        //enterComfortZone = false;
        //enterPanicZone = false;
        //enterShutdownZone = false;

        comfortZoneAttack = transform.GetChild(0).gameObject;
        panicZoneAttack = transform.GetChild(1).gameObject;
        shutdownZoneAttack = transform.GetChild(2).gameObject;

        zoneCollider = comfortZoneAttack.transform.GetComponent<Collider2D>();

        comfortZoneAttack.SetActive(false);
        panicZoneAttack.SetActive(false);
        shutdownZoneAttack.SetActive(false);

        newbieMovement = GetComponent<NewbieMovement>();
        trapAttack = panicZoneAttack.GetComponent<TrapAttack>();

        if (SceneManager.GetActiveScene().name == "combat" && GameMaster.commTriggered)
        {
            piggleCommunica = GameObject.FindGameObjectWithTag("PiggleSign").GetComponent<PiggleCommunica>();
            piggleCommunica.needRescue = false;
        }            
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "combat")
        {
            ZoneShift();
            DetectEnemyDensity();
        }
            

        // Debug
        //if (enterComfortZone && !enterPanicZone && !enterShutdownZone)
        //{
        //    comfortZoneAttack.SetActive(true);
        //    panicZoneAttack.SetActive(false);
        //    shutdownZoneAttack.SetActive(false);

        //}
        //else if (!enterComfortZone && enterPanicZone && !enterShutdownZone)
        //{
        //    comfortZoneAttack.SetActive(false);
        //    panicZoneAttack.SetActive(true);
        //    shutdownZoneAttack.SetActive(false);

        //}
        //else if (!enterComfortZone && !enterPanicZone && enterShutdownZone)
        //{
        //    comfortZoneAttack.SetActive(false);
        //    panicZoneAttack.SetActive(false);
        //    shutdownZoneAttack.SetActive(true);

        //}
        //else
        //{
        //    comfortZoneAttack.SetActive(false);
        //    panicZoneAttack.SetActive(false);
        //    shutdownZoneAttack.SetActive(false);         
        //}

    }

    private void ZoneShift()
    {
        HP = currentHP / maxHP;
        // Shift to panic
        if(CEDZ > CPborder * BEDZ * HP)
        {
            newbieMovement.stopMoving = true;
            if (GameMaster.commTriggered) piggleCommunica.needRescue = true;

            comfortZoneAttack.SetActive(false);
            panicZoneAttack.SetActive(true);

            
        }// Shift to beam attack
        else if (CEDZ <= CPborder * BEDZ * HP && !trapAttack.trapActive)
        {
            newbieMovement.stopMoving = false;
            if (GameMaster.commTriggered)
            {
                piggleCommunica.taskSolved = true;
                piggleCommunica.needRescue = false;
            }
            
            

            comfortZoneAttack.SetActive(true);
            panicZoneAttack.SetActive(false);

            
        }
    }

    private void DetectEnemyDensity()
    {
        CEDZ = 0;
        if(enemyList != null)
        {
            for (int i = 0; i < enemyList.Count; i++)
            {
                CEDZ += enemyList[i].transform.GetComponent<DamageableCharacter>().Density;
            }
        }
    }


}
