using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionZone : MonoBehaviour
{
    [Header("Detect Objects")]
    public List<Collider2D> detectObjects = new List<Collider2D>();

    private Collider2D collider;
    void Start()
    {
        collider = GetComponent<Collider2D>();
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
            detectObjects.Add(collision);
        }
        
    }

    // Detect when objects exit range
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            detectObjects.Remove(collision);
        }
        
    }
}
