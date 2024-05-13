using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CureReset : MonoBehaviour
{
    private bool startDeactive;
    void Start()
    {
        startDeactive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!startDeactive) StartCoroutine(SetActiveToFalse());

    }

    IEnumerator SetActiveToFalse()
    {
        startDeactive = true;
        yield return new WaitForSeconds(1.5f);
        this.gameObject.SetActive(false);
    }
}
