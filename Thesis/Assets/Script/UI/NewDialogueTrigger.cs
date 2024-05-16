using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewDialogueTrigger : MonoBehaviour
{
    private PlayerMovement player;
    private DialogueActivator dialogueActivator;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        dialogueActivator = GameObject.FindGameObjectWithTag("Newbie").GetComponent<DialogueActivator>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player.DialogueUI.dialogueEnd = false;
            dialogueActivator.dialogueTrigger = true;
            dialogueActivator.dialogueIndex = 1;
        }
    }
}
