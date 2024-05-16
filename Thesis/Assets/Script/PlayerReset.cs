using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReset : MonoBehaviour
{
    private GameMaster gm;
    private DamageableCharacter playerHealth;
    private DamageableCharacter piggleHealth;
    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();
        transform.position = gm.lastCheckPointPos;

        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<DamageableCharacter>();
        piggleHealth = GameObject.FindGameObjectWithTag("Newbie").GetComponent<DamageableCharacter>();
    }

    // Update is called once per frame
    void Update()
    {
        if (piggleHealth.Health > 0)
        {
            gm.playerZoomPos = transform.position;
        }
        else
        {
            gm.playerZoomPos = piggleHealth.gameObject.transform.position;
        }
        StartCoroutine(ResetTrigger());
    }

    private IEnumerator ResetTrigger()
    {
        yield return null;
        if (playerHealth.Health <= 0 || piggleHealth.Health <= 0)
        {
            yield return new WaitForSeconds(2);
            
            gm.DeathReset();
        } 
    }
}
