using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneAttack : MonoBehaviour
{
    [Header("Attack")]
    [SerializeField] private Transform firePoint;

    [Header("Zone")]
    [SerializeField] private bool enterComfortZone;
    [SerializeField] private bool enterPanicZone;
    [SerializeField] private bool enterShutdownZone;

    private GameObject comfortZoneAttack;
    private GameObject panicZoneAttack;
    private GameObject shutdownZoneAttack;

    void Start()
    {
        enterComfortZone = false;
        enterPanicZone = false;
        enterShutdownZone = false;

        comfortZoneAttack = transform.GetChild(0).gameObject;
        panicZoneAttack = transform.GetChild(1).gameObject;
        shutdownZoneAttack = transform.GetChild(2).gameObject;

        comfortZoneAttack.SetActive(false);
        panicZoneAttack.SetActive(false);
        shutdownZoneAttack.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (enterComfortZone && !enterPanicZone && !enterShutdownZone)
        {
            comfortZoneAttack.SetActive(true);
            panicZoneAttack.SetActive(false);
            shutdownZoneAttack.SetActive(false);

        }
        else if (!enterComfortZone && enterPanicZone && !enterShutdownZone)
        {
            comfortZoneAttack.SetActive(false);
            panicZoneAttack.SetActive(true);
            shutdownZoneAttack.SetActive(false);
            
        }
        else if (!enterComfortZone && !enterPanicZone && enterShutdownZone)
        {
            comfortZoneAttack.SetActive(false);
            panicZoneAttack.SetActive(false);
            shutdownZoneAttack.SetActive(true);
            
        }
        else
        {
            comfortZoneAttack.SetActive(false);
            panicZoneAttack.SetActive(false);
            shutdownZoneAttack.SetActive(false);         
        }

    }

}
