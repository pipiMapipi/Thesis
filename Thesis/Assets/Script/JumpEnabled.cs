using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class JumpEnabled : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(gameObject.activeSelf == true)
        {
            playerMovement.jumpEnabled = true;
        }
    }
}
