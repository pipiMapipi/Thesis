
using UnityEngine;

//[System.Serializable]
[CreateAssetMenu(menuName = "Dialogue/Question")]
public class Question : ScriptableObject
{
    [SerializeField] private string questionText;
    [SerializeField] private DialogueObject dialogueObject;

    //public string QuestionText => questionText;

    //public DialogueObject DialogueObject => dialogueObject;

    public string QuestionText
    {
        get => questionText;
        set
        {
            questionText = value;
        }
    }

    public DialogueObject DialogueObject
    {
        get => dialogueObject;
        set
        {
            dialogueObject = value;
        }
    }
}
