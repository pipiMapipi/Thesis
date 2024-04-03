using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyList : MonoBehaviour
{
    private ZoneAttack zoneAttack;
    void Start()
    {
        zoneAttack = transform.parent.transform.parent.GetComponent<ZoneAttack>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster"))
        {
            zoneAttack.enemyList.Add(collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster"))
        {
            zoneAttack.enemyList.Remove(collision);
        }
    }
}
