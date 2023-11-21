using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpNow : MonoBehaviour
{
    [SerializeField] private float newPosX;
    [SerializeField] private float initPosX;
    [SerializeField] private Transform player;
    [SerializeField] private PlayerMovement playerMovement;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (player.position.x > initPosX && playerMovement.jumpEnabled)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                player.position = new Vector2(newPosX, player.position.y);
            }
        }
    }
}
