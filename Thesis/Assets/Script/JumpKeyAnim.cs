using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpKeyAnim : MonoBehaviour
{
    [SerializeField] JumpNow jumpNowL;
    [SerializeField] JumpNow jumpNowR;

    private Animator jumpAnim;
    void Start()
    {
        jumpAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(jumpNowL.jumpEnabled || jumpNowR.jumpEnabled)
        {
            jumpAnim.SetBool("appear", true);
        } else if (!jumpNowL.jumpEnabled && !jumpNowR.jumpEnabled)
        {
            jumpAnim.SetBool("appear", false);
        }
    }
}
