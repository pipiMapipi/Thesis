using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingBomb : MonoBehaviour
{
    [Header("Bomb Movement")]
    [SerializeField] private float moveSpeed;

    private Vector2 direction;
    private GameObject player;
    private Rigidbody2D rb;


    void Start()
    {
        StartCoroutine(BombDestroy());
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        direction = (player.transform.position - transform.position).normalized;
        transform.Translate(direction * moveSpeed * Time.deltaTime);
    }

    private IEnumerator BombDestroy()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);
            Destroy(gameObject);
        }
    }
}
