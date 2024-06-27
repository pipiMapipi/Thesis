using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeCollapse : MonoBehaviour
{
    [SerializeField] bool isCrossed;
    
    private Animator collapseAnim;

    
    void Start()
    {
        collapseAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Newbie"))
        {
            if (isCrossed)
            {
                collapseAnim.SetTrigger("collapse");
                DamageableCharacter damageableCharacter = collision.GetComponent<DamageableCharacter>();
                damageableCharacter.Health -= 1000f;
                GameMaster.needBridge = true;
            }
            else
            {
                isCrossed = true;
            }
        }
        
        
    }
}
