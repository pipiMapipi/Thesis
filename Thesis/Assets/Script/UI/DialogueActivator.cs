using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogueActivator : MonoBehaviour, PlayerInteractable
{
    [SerializeField] private GameObject dialogueInstruct;
    [SerializeField] private List<DialogueObject> dialogueObject = new List<DialogueObject>();  
    public int dialogueIndex;

    [Header("Questions")]
    [SerializeField] private List<Question> questions = new List<Question>();
    [SerializeField] private List<bool> questionActive = new List<bool>();
    List<Question> questionsToSet = new List<Question>();

    [Header("Emotion")]
    public bool dialogueTrigger;
    [SerializeField] private Transform emotions;

    private Animator anim;
    private bool withinReach;

    private NewbieMovement piggle;
    private PlayerMovement player;

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "Reborn")
        {
            questionActive[0] = GameMaster.needCommunication;
            questionActive[1] = GameMaster.needSlime;
            questionActive[2] = GameMaster.needJump;
            questionActive[3] = GameMaster.needFountain;
            questionActive[4] = GameMaster.needBridge;
        }

        anim = dialogueInstruct.GetComponent<Animator>();


        for(int i = 0; i < questionActive.Count; i++)
        {
            if (questionActive[i])
            {
                questionsToSet.Add(questions[i]);
            }
        }

        //if (SceneManager.GetActiveScene().name == "Dialogue") {
        //    dialogueIndex = 0;
        //}
        

        piggle = GameObject.FindGameObjectWithTag("Newbie").GetComponent<NewbieMovement>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();

        if(dialogueObject[dialogueIndex].RootDialogue && dialogueObject[dialogueIndex].Questions.Count > 1)
        {
            dialogueObject[dialogueIndex].Questions.RemoveRange(1, dialogueObject[dialogueIndex].Questions.Count-1);
        }
    }

    private void Update()
    {
        if (dialogueTrigger)
        {
            emotions.GetChild(0).gameObject.SetActive(true);
            if (player.DialogueUI.dialogueEnd && dialogueIndex == 0) EndDialogue();
        }
        else
        {
            for (int i = 0; i < emotions.childCount; i++)
            {
                emotions.GetChild(i).gameObject.SetActive(false);
            }
        }

        
    }

    public void UpdateDialogueObject(DialogueObject dialogueObject)
    {
        this.dialogueObject[dialogueIndex] = dialogueObject;
    }

    public void Interact(PlayerMovement player)
    {
        foreach(DialogueResponseEvents responseEvents in GetComponents<DialogueResponseEvents>())
        {
            if (responseEvents.DialogueObject == dialogueObject[dialogueIndex])
            {
                player.DialogueUI.AddReponseEvents(responseEvents.Events);
                break;
            }
        }
        if(dialogueObject[dialogueIndex].Questions.Count == 1 && dialogueObject[dialogueIndex].RootDialogue)
        {
            //questionsToSet.Add(dialogueObject[dialogueIndex].Questions[0]);
            //dialogueObject[dialogueIndex].Questions = questionsToSet.ToArray();
            dialogueObject[dialogueIndex].Questions.AddRange(questionsToSet);


        }
            

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

   public void EndDialogue()
    {
        dialogueTrigger = false;
        if (SceneManager.GetActiveScene().name == "Dialogue" && dialogueIndex == 0)
        {
            piggle.stopMoving = false;
            
        }
    }
}
