using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class ScoreControll : MonoBehaviour
{
    [Header("Headshots")]
    [SerializeField] private List<Sprite> headShots = new List<Sprite>();
    [SerializeField] private Image headShot;

    [Header("Multichoice")]
    [SerializeField] Transform takedown;
    [SerializeField] Transform healing;

    [Header("Final Score")]
    [SerializeField] private List<Sprite> scores = new List<Sprite>();
    [SerializeField] private Image score;

    [Header("Character Anim")]
    [SerializeField] private List<Animator> charAnims = new List<Animator>();

    [Header("Reset Anim")]
    [SerializeField] private Animator resetAnim;

    private bool playerNow;
    private bool hasChanged;

    private float takeDownWeight = 3f;
    private float healingWeight = 2f;
    private float takeDownRatio;
    private float healingRatio;

    private Animator anim;
    private float revealTime;
    private bool startReveal;

     


    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(revealTime > 7f)
        {
            resetAnim.SetTrigger("reveal");
        }
        else
        {
            if(startReveal) revealTime += Time.deltaTime;
        }

        if (!playerNow)
        {
            UpdateHeadshot(0);
            CheckTakedown(GameMaster.slimePiggle);
            CheckHealing(GameMaster.potionPiggle, GameMaster.potionCanDrop);
            CheckFinalScore(takeDownRatio, healingRatio);
        }
        else
        {
            if (!hasChanged)
            {
                anim.SetTrigger("next");
                charAnims[0].SetTrigger("next");
                StartCoroutine(UpdateAvatar());
                hasChanged = true;
                startReveal = true;
            }
            
        }
    }

    public void ChangeToPlayer()
    {
        playerNow = true;
    }

    private IEnumerator UpdateAvatar()
    {
        yield return new WaitForSeconds(1f);
        charAnims[1].SetTrigger("next");
        yield return new WaitForSeconds(1f);
        UpdateHeadshot(1);
        UncheckChoices();
        CheckTakedown(GameMaster.slimePlayer);
        CheckHealing(GameMaster.potionPlayer, 4);
        CheckFinalScore(takeDownRatio, healingRatio);
    }

    void UpdateHeadshot(int index)
    {
        headShot.sprite = headShots[index];
    }

    void UncheckChoices()
    {
        for (int i = 0; i < takedown.childCount; i++)
        {
            takedown.GetChild(i).GetChild(1).gameObject.SetActive(false);
            healing.GetChild(i).GetChild(1).gameObject.SetActive(false);
        }
    }
    void CheckTakedown(int slimeTakedown)
    {
        float ratio = (float) slimeTakedown / (GameMaster.slimeSum - 3f);
        takeDownRatio = ratio;

        if (ratio >= 0.9f)
        {
            takedown.GetChild(0).GetChild(1).gameObject.SetActive(true);
        } // A
        else if (ratio < 0.9f && ratio >= 0.75f)
        {
            takedown.GetChild(1).GetChild(1).gameObject.SetActive(true);
        } // B
        else if (ratio < 0.75f && ratio >= 0.5f)
        {
            takedown.GetChild(2).GetChild(1).gameObject.SetActive(true);
        } // C
        else
        {
            takedown.GetChild(3).GetChild(1).gameObject.SetActive(true);
        } // F
    }
    
    void CheckHealing(int potionGet, int potionSum)
    {
        float ratio = (float) potionGet / potionSum;
        healingRatio = ratio;

        if (ratio >= 0.9f)
        {
            healing.GetChild(0).GetChild(1).gameObject.SetActive(true);
        } // A
        else if (ratio < 0.9f && ratio >= 0.75f)
        {
            healing.GetChild(1).GetChild(1).gameObject.SetActive(true);
        } // B
        else if (ratio < 0.75f && ratio >= 0.5f)
        {
            healing.GetChild(2).GetChild(1).gameObject.SetActive(true);
        } // C
        else
        {
            healing.GetChild(3).GetChild(1).gameObject.SetActive(true);
        } // F
    }

    void CheckFinalScore(float _takedownRatio, float _healthRatio)
    {
        float ratio = ((_takedownRatio * takeDownWeight) + (_healthRatio * healingWeight)) / (takeDownWeight + healingWeight);
        if (ratio >= 0.8f)
        {
            score.sprite = scores[0];
        } // A
        else if (ratio < 0.8f && ratio >= 0.6f)
        {
            score.sprite = scores[1];
        } // B
        else if (ratio < 0.6f && ratio >= 0.35f)
        {
            score.sprite = scores[2];
        } // C
        else
        {
            score.sprite = scores[3];
        } // F
    }
}
