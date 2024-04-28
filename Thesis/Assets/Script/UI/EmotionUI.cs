using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmotionUI : MonoBehaviour
{
    private Animator baseAnim;
    private Transform emoDetail;
    private Animator emoAnim;
    void Start()
    {
        baseAnim = transform.GetChild(0).GetComponent<Animator>();
        emoDetail = transform.GetChild(1);
        emoAnim = emoDetail.GetComponent<Animator>();

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CurrentEmotion(int emoCategory)
    {
        for (int i = 0; i < emoDetail.childCount; i++)
        {
            if (i == emoCategory)
            {
                emoDetail.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                emoDetail.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    public void AnimFadeOut()
    {
        baseAnim.SetTrigger("end");
        emoAnim.SetTrigger("end");
    }
}
