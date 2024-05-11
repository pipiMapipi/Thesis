using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommunictionIconTrigger : MonoBehaviour
{
    private Transform player;
    private bool dropSignEnabled;
    private Transform piggle;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        dropSignEnabled = true;

        piggle = GameObject.FindGameObjectWithTag("Newbie").transform;
    }

    // Update is called once per frame
    void Update()
    {
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
    }
}
