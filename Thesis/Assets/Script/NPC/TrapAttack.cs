using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapAttack : MonoBehaviour
{
    public bool trapActive;

    [SerializeField] private GameObject seedAttack;
    [SerializeField] private Vector2 groundDispenseVel;
    [SerializeField] private Vector2 verticalDispenseVel;

    [SerializeField] private Camera cam;

    

    private Quaternion rotation;

    private int seedCount = 4;
    private List<Vector2> seedPositionList = new List<Vector2>();
    private bool hasResetList;
    private bool hasInitAttack;

    void OnEnable()
    {
        hasInitAttack = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasInitAttack)
        {
            StartCoroutine(InitSeedAttack());
        }
        
    }

    void Fire()
    {
        GameObject instantiateSeed = Instantiate(seedAttack, transform.position, Quaternion.identity);
        instantiateSeed.GetComponent<FakeHeight>().Initialize(transform.right * Random.Range(groundDispenseVel.x, groundDispenseVel.y), Random.Range(verticalDispenseVel.x, verticalDispenseVel.y));
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
            
        //}
    }

    private void RotateToTarget(Vector2 seedPos)
    {
        //Vector2 direction = cam.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        Vector2 direction = seedPos - (Vector2)transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rotation.eulerAngles = new Vector3(0, 0, angle);
        transform.rotation = rotation;
    }

    private void NewSeedList()
    {
        hasResetList = true;
        seedPositionList.Clear();
        for(int i = 0; i < seedCount; i++)
        {
            Vector2 seedPos = new Vector2(Random.Range(-10, 10) + transform.position.x, Random.Range(-10, 10) + transform.position.y);
            seedPositionList.Add(seedPos);
        }
        
    }

    private IEnumerator InitSeedAttack()
    {
        trapActive = true;
        hasInitAttack = true;
        if (!hasResetList)
        {
            NewSeedList();
        }
        if(seedPositionList != null)
        {
            for(int i = 0; i < seedPositionList.Count; i++)
            {
                RotateToTarget(seedPositionList[i]);
                Fire();
            }
        }


        yield return new WaitForSeconds(10f);
        trapActive = false;
        hasResetList = false;
        hasInitAttack = false;
    }
}
