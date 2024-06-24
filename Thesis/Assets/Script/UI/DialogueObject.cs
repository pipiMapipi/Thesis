using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/DialogueObject")]
public class DialogueObject : ScriptableObject
{
    [SerializeField] [TextArea] private string[] dialogue;
    [SerializeField] private List<Question> questions = new List<Question>();
    [SerializeField] private bool rootDialogue;

    public string[] Dialogue => dialogue; // Read not overwrite

    public bool HasQuestions => questions != null && questions.Count > 0;

    public List<Question> Questions
    {
        get => questions;
        set
        {
            questions = value;
        }
    }

    public bool RootDialogue
    {
        get => rootDialogue;
        set
        {
            RootDialogue = value;
        }
    }
}
