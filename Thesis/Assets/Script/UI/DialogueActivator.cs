using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueActivator : MonoBehaviour, PlayerInteractable
{
    [SerializeField] private GameObject dialogueInstruct;
    [SerializeField] private List<DialogueObject> dialogueObject = new List<DialogueObject>();
    public int dialogueIndex;

    [Header("Emotion")]
    public bool dialogueTrigger;
    [SerializeField] private Transform emotions;

    private Animator anim;
    private bool withinReach;

    private void Start()
    {
        anim = dialogueInstruct.GetComponent<Animator>();
    }

    private void Update()
    {
        if (dialogueTrigger)
        {
            if (withinReach)
            {
                dialogueInstruct.SetActive(true);
                for(int i = 0; i < emotions.childCount; i++)
                {
                    emotions.GetChild(i).gameObject.SetActive(false);
                }
                
            }
            else
            {

            }
        }
        else
        {
            StartCoroutine(EndInstruct());
            for (int i = 0; i < emotions.childCount; i++)
            {
                emotions.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
    public void Interact(PlayerMovement player)
    {
        player.DialogueUI.ShowDialogue(dialogueObject[dialogueIndex]);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //check if it has the player component
        if (collision.CompareTag("Player") && collision.TryGetComponent(out PlayerMovement player)) 
        {
            player.Interactable = this;
            withinReach = true;
            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && collision.TryGetComponent(out PlayerMovement player))
        {
            if (player.Interactable is DialogueActivator dialogueActivator && dialogueActivator == this)
            {
                player.Interactable = null;
                //StartCoroutine(EndInstruct());
                withinReach = false;
            }
        }
    }

    private IEnumerator EndInstruct()
    {
        anim.SetTrigger("Talk");
        yield return new WaitForSeconds(0.4f);
        dialogueInstruct.SetActive(false);
    }
}
