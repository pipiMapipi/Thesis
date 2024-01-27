using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReset : MonoBehaviour
{
    private GameMaster gm;
    private DamageableCharacter playerHealth;
    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();
        transform.position = gm.lastCheckPointPos;

        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<DamageableCharacter>();
    }

    // Update is called once per frame
    void Update()
    {
        gm.playerZoomPos = transform.position;
        StartCoroutine(ResetTrigger());
    }

    private IEnumerator ResetTrigger()
    {
        yield return null;
        if (playerHealth.Health == 0)
        {
            yield return new WaitForSeconds(2);
            
            gm.DeathReset();
        }
    }
}
