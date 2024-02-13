using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Newbie : MonoBehaviour
{
    public bool isCarrying;
    [SerializeField] private CheckPlayerInReach checkPlayerInReach;
    [SerializeField] private float yOffset = -0.5f;

    [Header("Weapon")]
    [SerializeField] private GameObject weapon;
    [SerializeField] private float attackInterval = 5f;

    private GameObject player;
    private Vector3 dropPos;
    private bool weaponActivate;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        weapon.SetActive(false);

        
    }

    // Update is called once per frame
    void Update()
    {
        if (isCarrying)
        {
            CarryNPC();
        }

        if(!isCarrying || (isCarrying && (Input.GetKeyDown(KeyCode.B))))
        {
            if (!weaponActivate)
            {
                weaponActivate = true;
                StartCoroutine(ShieldDamage());
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isCarrying)
            {
                if (checkPlayerInReach.playerInReach.Count != 0)
                {
                    isCarrying = true;
                }
            }
            else
            {
                dropPos = player.transform.position;
                DropNPC();
            }
        }
    }

    private void CarryNPC()
    {

        this.transform.position = player.transform.position - new Vector3(0, - yOffset, 0);
    }

    private void DropNPC()
    {
        isCarrying = false;
        checkPlayerInReach.playerInReach.Clear();
        this.transform.position = dropPos;
    }

    private IEnumerator ShieldDamage()
    {
        yield return new WaitForSeconds(attackInterval);
        weapon.SetActive(true);

        yield return new WaitForSeconds(2);
        weapon.SetActive(false);
        weaponActivate = false;
    }
}
