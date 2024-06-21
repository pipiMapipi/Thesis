using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class QuestionHandler : MonoBehaviour
{
    [SerializeField] private RectTransform questionButtonTemplate;
    [SerializeField] private GameObject questionContainer;

    [SerializeField] DialogueUI dialogueUI;

    private List<GameObject> tempQuestionButtons = new List<GameObject>();
    private bool animStarted;
    private int index = 0;

    private ResponseEvent[] responseEvents;

    private void Update()
    {
        if (tempQuestionButtons.Count != 0)
        {
            if (!animStarted) StartCoroutine(TriggerAnim());
        }
        else
        {
            index = 0;
            animStarted = false;
        }
    }

    public void AddResponseEvents(ResponseEvent[] responseEvents)
    {
        this.responseEvents = responseEvents;
    }

    public void ShowQuestions(Question[] questions)
    {
        
       
        for(int i = 0; i < questions.Length; i++)
        {
            Question question = questions[i];
            int currentIndex = i;

            GameObject questionButton = Instantiate(questionButtonTemplate.gameObject, questionContainer.transform.GetChild(i));
            questionButton.gameObject.SetActive(true);
            
            questionButton.transform.GetChild(0).GetComponent<TMP_Text>().text = question.QuestionText;

            
            questionButton.GetComponent<Button>().onClick.AddListener(() => OnPickedQuestion(question, currentIndex));

            tempQuestionButtons.Add(questionButton);
        }
    }

    private void OnPickedQuestion(Question question, int questionIndex)
    {
        //questionContainer.gameObject.SetActive(false);

        foreach (GameObject button in tempQuestionButtons)
        {
            Destroy(button);
        }
        tempQuestionButtons.Clear();

        if(responseEvents != null && questionIndex <= responseEvents.Length)
        {
            responseEvents[questionIndex].OnPickedResponse?.Invoke();
        }

        responseEvents = null;

        if (question.DialogueObject)
        {
            dialogueUI.ShowDialogue(question.DialogueObject);
        }
        else
        {
            dialogueUI.CloseDialogueBox();
        }
        
    }

    private IEnumerator TriggerAnim()
    {
        animStarted = true;
        while (index < tempQuestionButtons.Count)
        {
            yield return new WaitForSeconds(0.3f);
            tempQuestionButtons[index].GetComponent<Animator>().SetTrigger("PopUp");
            index += 1;
        }
    }
}
