using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewDialogueTrigger : MonoBehaviour
{
    private PlayerMovement player;
    public DialogueActivator dialogueActivator;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && SceneManager.GetActiveScene().name == "Dialogue")
        {
            player.DialogueUI.dialogueEnd = false;
            dialogueActivator.dialogueTrigger = true;
            dialogueActivator.dialogueIndex = 1;
        }
    }
}
