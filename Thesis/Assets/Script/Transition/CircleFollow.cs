using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleFollow : MonoBehaviour
{
    //private DamageableCharacter playerHealth;
    //private GameObject player;
    private GameMaster gm;
    // Start is called before the first frame update
    void Start()
    {
        //player = GameObject.FindGameObjectWithTag("Player");
        //playerHealth = player.GetComponent<DamageableCharacter>();
        gm = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = gm.playerZoomPos;
    }
}
