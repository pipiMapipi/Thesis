using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [Header("Weapon")]
    [SerializeField] private GameObject weaponPrefab;
    [SerializeField] private float attackInterval;
    void Start()
    {
        StartCoroutine(InitBombPrefab());
    }


    void Update()
    {

    }


    private IEnumerator InitBombPrefab()
    {
        while (true)
        {
            yield return new WaitForSeconds(attackInterval);
            Instantiate(weaponPrefab, this.transform);
        }
    }

}
