
using UnityEngine;

[System.Serializable]
public class Question
{
    [SerializeField] private string questionText;
    [SerializeField] private DialogueObject dialogueObject;

    public string QuestionText => questionText;

    public DialogueObject DialogueObject => dialogueObject;
}
