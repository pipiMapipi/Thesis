using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueActivator : MonoBehaviour, PlayerInteractable
{
    [SerializeField] private DialogueObject dialogueObject;
    public void Interact(PlayerMovement player)
    {
        player.DialogueUI.ShowDialogue(dialogueObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //check if it has the player component
        if (collision.CompareTag("Player") && collision.TryGetComponent(out PlayerMovement player)) 
        {
            player.Interactable = this;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && collision.TryGetComponent(out PlayerMovement player))
        {
            if (player.Interactable is DialogueActivator dialogueActivator && dialogueActivator == this)
            {
                player.Interactable = null;
            }
        }
    }
}
