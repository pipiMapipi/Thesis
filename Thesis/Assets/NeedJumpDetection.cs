using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeedJumpDetection : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameMaster.needJump = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Newbie"))
        {
            GameMaster.needJump = true;
        }
    }
}
