using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TMP_Text dialogueText;

    [SerializeField] private QuestionHandler questionHandler;

    [SerializeField] private Animator instructAnim;

    public bool IsOpen { get; private set; } // only this script can set whether it's open
    public bool dialogueEnd;

    private TypeWriter typeWriter;

    public DialogueActivator dialogueActivator;
    
    void Start()
    {
        typeWriter = GetComponent<TypeWriter>();
        CloseDialogueBox();

       
    }

    public void ShowDialogue(DialogueObject dialogueObject)
    {
        IsOpen = true;
        //instructAnim.SetTrigger("Talk");
        dialogueBox.SetActive(true);
        StartCoroutine(StepThroughDialogue(dialogueObject));
    }

    public void AddReponseEvents(ResponseEvent[] responseEvents)
    {
        questionHandler.AddResponseEvents(responseEvents);
    }

    private IEnumerator StepThroughDialogue(DialogueObject dialogueObject)
    {

        for (int i = 0; i < dialogueObject.Dialogue.Length; i++)
        {
            
            string dialogue = dialogueObject.Dialogue[i];
            yield return RunTypingEffect(dialogue);
            dialogueText.text = dialogue;

            if (i == dialogueObject.Dialogue.Length - 1 && dialogueObject.HasQuestions) break;

            yield return null;
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift));
        }

        if (dialogueObject.HasQuestions)
        {
            questionHandler.ShowQuestions(dialogueObject.Questions);
        }
        else
        {
            CloseDialogueBox();
            dialogueEnd = true;
        }
        
    }

    private IEnumerator RunTypingEffect(string dialogue)
    {
        typeWriter.TextRun(dialogue, dialogueText);

        while (typeWriter.IsRunning)
        {
            yield return null;

            if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
            {
                typeWriter.TextStop();
            }
        }
    }

    public void CloseDialogueBox()
    {
        IsOpen = false;
        dialogueBox.SetActive(false);
        dialogueText.text = string.Empty;
    }
}
