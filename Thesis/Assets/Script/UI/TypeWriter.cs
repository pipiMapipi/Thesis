using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TypeWriter : MonoBehaviour
{
    [SerializeField] private float typeSpeed = 50f;

    public bool IsRunning { get; private set; }

    // Punctuation speed
    private readonly Dictionary<HashSet<char>, float> punctuations = new Dictionary<HashSet<char>, float>()
    {
        { new HashSet<char>() {'.', '!', '?'}, 0.6f},
        { new HashSet<char>() {',', ';', ':'}, 0.3f},
    };

    private Coroutine typingCoroutine;

    public void TextRun(string textToType, TMP_Text dialogueText)
    {
        typingCoroutine = StartCoroutine(TypeText(textToType, dialogueText));
    }

    public void TextStop()
    {
        StopCoroutine(typingCoroutine);
        IsRunning = false;
    }

    private IEnumerator TypeText(string textToType, TMP_Text dialogueText)
    {
        IsRunning = true;

        dialogueText.text = string.Empty;

        float time = 0;
        int charIndex = 0;

        while(charIndex < textToType.Length)
        {
            int lastCharIndex = charIndex;

            time += Time.deltaTime * typeSpeed;
            charIndex = Mathf.FloorToInt(time);
            charIndex = Mathf.Clamp(charIndex, 0, textToType.Length);

            // Keep framerate consistent
            for (int i = lastCharIndex; i < charIndex; i++)
            {
                bool isLast = i >= textToType.Length - 1;
                dialogueText.text = textToType.Substring(0, i+1);

                if(IsPunctuation(textToType[i], out float waitTime) && !isLast && !IsPunctuation(textToType[i+1], out _))
                {
                    yield return new WaitForSeconds(waitTime);
                }
            }
           

            yield return null;
        }

        IsRunning = false;
        dialogueText.text = textToType;
    }

    private bool IsPunctuation(char character, out float waitTime)
    {
        foreach (KeyValuePair<HashSet<char>, float> punctuationCategory in punctuations)
        {
            if (punctuationCategory.Key.Contains(character))
            {
                waitTime = punctuationCategory.Value;
                return true;
            }
        }

        waitTime = default;
        return false;
    }
}

