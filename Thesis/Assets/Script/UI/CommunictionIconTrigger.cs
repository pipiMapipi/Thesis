using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommunictionIconTrigger : MonoBehaviour
{
    public bool signDropped;
    public Transform signPos;
    public bool clearSign;
    public float piggleWaitTime;

    private Transform player;
    private bool dropSignEnabled;

    
    private Transform Piggle;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        Piggle = GameObject.FindGameObjectWithTag("Newbie").transform;
        dropSignEnabled = true;

    }

    // Update is called once per frame
    void Update()
    {
        if (signDropped && Vector2.Distance(Piggle.position, signPos.position) < 3f)
        {
            if (piggleWaitTime > 3f)
            {
                clearSign = true;
            }
            else
            {
                piggleWaitTime += Time.deltaTime;
            }
        }

        if (dropSignEnabled)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                dropSignEnabled = false;
                DropASign(0, player.position);
            }else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                dropSignEnabled = false;
                DropASign(1, player.position);
            }else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                dropSignEnabled = false;
                DropASign(2, player.position);
            }
        }
    }

    private void DropASign(int index, Vector2 currentPos)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (i != index) transform.GetChild(i).gameObject.SetActive(false);
        }
        Transform thisSign = transform.GetChild(index);
        thisSign.gameObject.SetActive(true);
        thisSign.position = currentPos;
        dropSignEnabled = true;
        signDropped = true;
        signPos = thisSign; ;
    }
}
