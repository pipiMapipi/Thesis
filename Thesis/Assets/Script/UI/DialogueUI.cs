using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TMP_Text dialogueText;

    [SerializeField] private QuestionHandler questionHandler;

    public bool IsOpen { get; private set; } // only this script can set whether it's open

    private TypeWriter typeWriter;
    
    void Start()
    {
        typeWriter = GetComponent<TypeWriter>();
        CloseDialogueBox();
    }

    public void ShowDialogue(DialogueObject dialogueObject)
    {
        IsOpen = true;
        dialogueBox.SetActive(true);
        StartCoroutine(StepThroughDialogue(dialogueObject));
    }

    private IEnumerator StepThroughDialogue(DialogueObject dialogueObject)
    {

        foreach (string dialogue in dialogueObject.Dialogue)
        {
            
        }
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

    private void CloseDialogueBox()
    {
        IsOpen = false;
        dialogueBox.SetActive(false);
        dialogueText.text = string.Empty;
    }
}
