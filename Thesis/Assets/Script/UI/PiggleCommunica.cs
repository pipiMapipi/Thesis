using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiggleCommunica : MonoBehaviour
{
    public bool needClearFlower;
    public Vector2 flowerPos;
    public bool needRescue;
    public bool taskSolved;

    private float offset = 1f;

    private Transform piggle;
    private bool dropSignEnabled;


    void Start()
    {
        piggle = GameObject.FindGameObjectWithTag("Newbie").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (dropSignEnabled)
        {
            if (needClearFlower)
            {
                dropSignEnabled = false;
                taskSolved = false;
                DropASign(0, flowerPos);
            }
            if (needRescue)
            {
                dropSignEnabled = false;
                taskSolved = false;
                DropASign(1, (Vector2) piggle.position + new Vector2(0, offset));
            }
        }
        if(taskSolved)
        {
            if (!needClearFlower) StartCoroutine(DeleteSign(0));
            if (!needRescue) StartCoroutine(DeleteSign(1));
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

    private IEnumerator DeleteSign(int index)
    {
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < transform.childCount; i++)
        {
            if (i == index) transform.GetChild(i).gameObject.SetActive(false);
        }
        dropSignEnabled = true;
    }
}
