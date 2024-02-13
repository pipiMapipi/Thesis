using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPlayerInReach : MonoBehaviour
{

    [Header("Detect Objects")]
    public List<Collider2D> playerInReach = new List<Collider2D>();

    [SerializeField] Newbie newbie;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
      
    }

    // Detect when objects enter range
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInReach.Add(collision);
        }

    }

}
