using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineDetectZone : MonoBehaviour
{
    public Animator anim;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Newbie"))
        {
            anim.enabled = true;
            GameMaster.machineIsGone = true;
        }
    }
}
