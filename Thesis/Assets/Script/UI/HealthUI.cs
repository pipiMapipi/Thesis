using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private Image healthBar;
    [SerializeField] private Sprite[] lifeIcon;
    [SerializeField] private GameObject maxLife;

    [Header("Piggle")]
    [SerializeField] private Image healthBarPiggle;

    private DamageableCharacter playerHealth;
    private float oneLifeHealth;
    private int lifeNow;
    private int lifeCount;
    private bool lifeChange;
    private float preHealth;

    private DamageableCharacter piggleHealth;
    void Awake()
    {
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<DamageableCharacter>();
        lifeCount = maxLife.transform.childCount;
        preHealth = playerHealth.Health;

        piggleHealth = GameObject.FindGameObjectWithTag("Newbie").GetComponent<DamageableCharacter>();
    }

    // Update is called once per frame
    void Update()
    {
        
        oneLifeHealth = playerHealth.MaxHealth / 3f;

        StartCoroutine(healthUI());

        if (playerHealth.Health % oneLifeHealth == 0 && playerHealth.Health != preHealth)
        {
            lifeChange = true;
        }

        if (lifeChange)
        {
            StartCoroutine(LifeUI());
        }

        preHealth = playerHealth.Health;

        healthBarPiggle.fillAmount = piggleHealth.Health / piggleHealth.MaxHealth;
    }

    private IEnumerator healthUI()
    {
        
        
        if(playerHealth.Health != 0)
        {
            if (playerHealth.Health % oneLifeHealth == 0)
            {
                healthBar.fillAmount = 1;
            }
            else
            {
                healthBar.fillAmount = (playerHealth.Health % oneLifeHealth) / oneLifeHealth;
            }
        }else
        {
            healthBar.fillAmount = 0;
        }

        yield return null;
    }

    private IEnumerator LifeUI()
    {
        for(int i = 0; i < lifeCount; i++)
        {
            maxLife.transform.GetChild(i).GetComponent<Image>().sprite = lifeIcon[0];
        }
        lifeNow = Mathf.CeilToInt(playerHealth.Health / oneLifeHealth);

        if(lifeNow > 0)
        {
            for (int i = 0; i < lifeNow; i++)
            {
                maxLife.transform.GetChild(i).GetComponent<Image>().sprite = lifeIcon[1];
            }
        }

        yield return null;
    }
}
