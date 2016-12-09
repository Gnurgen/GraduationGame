using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class PlayerHealthBar : MonoBehaviour {

    // ENEMY TRACKER
    [SerializeField]
    private float percentageToKill;
    [SerializeField]
    private GameObject guidingWhisp;

    [SerializeField]
    private float hpRegenSpeed = .1f, sRegenSpeed = .1f;
    [SerializeField]
    private bool FlashRedFull = false, FlashWhiteFull;
    [SerializeField]
    private int HPFlashFrames, SpiritFlashFrames;
    private int allEnemies;
    private int currentEnemiesAlive;
    private bool guideSpawned;


    [SerializeField]
    private Image hpBar, hpBar_Fill, hpBar_Flash, sBar, sBar_Fill, sBar_Flash;

    //Private stuff
    private float currentVal, maxVal;
    private GameObject player;
    private float spiritScale = 0, aniSpiritScale = 0;
    private float hpScale = 1, aniHPScale = 1;
    private float minSize = 0;
    private float maxSize = 1;

    //Input manager and Event manager

    void Start() {

        sBar_Flash.fillAmount = 0;
        hpBar_Flash.fillAmount = 0;
        player = GameManager.player;

        GameManager.events.OnLoadComplete += GetTotalEnemies;
        GameManager.events.OnEnemyAttackHit += TakeDamage;
        GameManager.events.OnResourcePickup += SpiritPickUp;

        maxVal = currentVal = player.GetComponent<Health>().health;

        guideSpawned = GameManager.progress > GameManager.numberOfLevels;
    }



    void Update()
    {
        if (hpScale != aniHPScale) // HP
        {
            if (Mathf.Abs(aniHPScale - hpScale) > hpRegenSpeed * Time.deltaTime * 0.5f)
            {
                if (hpScale < aniHPScale) // DAMAGE TAKEN !
                {
                    aniHPScale = aniHPScale - (hpRegenSpeed * Time.deltaTime);
                    hpBar.fillAmount = hpScale;
                    hpBar_Fill.color = Color.red;
                    hpBar_Fill.fillAmount = aniHPScale;
                }
                else if (hpScale > aniHPScale) // HEALING
                {
                    aniHPScale = aniHPScale + (hpRegenSpeed * Time.deltaTime);
                    hpBar.fillAmount = aniHPScale;
                    hpBar_Fill.color = Color.grey; // DarkGreen
                    hpBar_Fill.fillAmount = hpScale;
                }
            }
            else    // DO nothing
            {
                aniHPScale = hpScale;
                hpBar_Fill.color = Color.white; //hpBar's color
            }
        }
        if (spiritScale > aniSpiritScale) // SPIRIT BAR
        {
            aniSpiritScale += sRegenSpeed * Time.deltaTime;
            sBar_Fill.fillAmount = spiritScale;
            sBar.fillAmount = aniSpiritScale;
        }
        else
        {
            aniSpiritScale = spiritScale;
        }
    }
    

    private void GetTotalEnemies()
    {
        allEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
        currentEnemiesAlive = allEnemies;
    }

    private void SpiritPickUp(GameObject GO, int amount)
    {
        StartCoroutine(UpdatePickUp());
    }
    private void TakeDamage(GameObject enemyID, float dmg)
    {
        StartCoroutine(UpdateDamage());
    }

    private IEnumerator UpdatePickUp()
    {
        yield return new WaitForEndOfFrame();
        //HP 
        currentVal = player.GetComponent<Health>().health;
        hpScale = 1 - ((maxVal - currentVal) / maxVal);

        //Spirit
        currentEnemiesAlive -= 1;
        if (percentageToKill != 0)
            spiritScale = (1 - (float)currentEnemiesAlive / allEnemies) / percentageToKill;
        if (spiritScale > 1)
            spiritScale = 1;
        if (!guideSpawned && ((float)currentEnemiesAlive / (float)allEnemies) <= (1 - percentageToKill))
        {
            Instantiate(guidingWhisp, GameManager.spear.transform.position, Quaternion.identity);
            guideSpawned = true;
        }

        if (FlashWhiteFull)
            sBar_Flash.fillAmount = 1;
        else
            sBar_Flash.fillAmount = spiritScale;
        sBar_Flash.color = Color.white;
        int frame = SpiritFlashFrames;
        while (frame > 0)
        {
            Color col = new Color(1, 1, 1, (float)frame / SpiritFlashFrames);
            sBar_Flash.color = col;
            frame--;
            yield return null;
        }
        hpBar_Flash.color = Color.clear;
    }
    
  
    private IEnumerator UpdateDamage()
    {
        yield return new WaitForEndOfFrame();
        currentVal = player.GetComponent<Health>().health;
        hpScale = 1 - ((maxVal - currentVal) / maxVal);

        if(FlashRedFull)
            hpBar_Flash.fillAmount = 1;
        else
            hpBar_Flash.fillAmount = aniHPScale;
        hpBar_Flash.color = Color.red;
        int frame = HPFlashFrames;
        while (frame > 0)
        {
            Color col = new Color(1, 0, 0, (float)frame /HPFlashFrames);
            hpBar_Flash.color = col;
            frame--;
            yield return null;
        }
        hpBar_Flash.color = Color.clear;

    }

    public float GetProgress()
    {
        return spiritScale;
    }
    
}
