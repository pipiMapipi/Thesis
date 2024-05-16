using System;
using UnityEngine;

public class DialogueResponseEvents : MonoBehaviour
{
    [SerializeField] private DialogueObject dialogueObject;
    [SerializeField] private ResponseEvent[] events;

    public DialogueObject DialogueObject => dialogueObject;
    public ResponseEvent[] Events => events;

    public void OnValidate()
    {
        if (dialogueObject == null) return;
        if (dialogueObject.Questions == null) return;
        if (events != null && events.Length == dialogueObject.Questions.Length) return;

        if(events == null)
        {
            events = new ResponseEvent[dialogueObject.Questions.Length];
        }
        else
        {
            Array.Resize(ref events, dialogueObject.Questions.Length);
        }

        for(int i = 0; i < dialogueObject.Questions.Length; i++)
        {
            Question question = dialogueObject.Questions[i];

            if(events[i] != null)
            {
                events[i].name = question.QuestionText;
                continue;
            }

            events[i] = new ResponseEvent() { name = question.QuestionText };
        }
    }
}
