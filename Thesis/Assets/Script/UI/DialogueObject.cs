using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/DialogueObject")]
public class DialogueObject : ScriptableObject
{
    [SerializeField] [TextArea] private string[] dialogue;
    [SerializeField] private Question[] questions;

    public string[] Dialogue => dialogue; // Read not overwrite

    public bool HasQuestions => questions != null && questions.Length > 0;

    public Question[] Questions
    {
        get => questions;
        set
        {
            questions = value;
        }
    }
}
