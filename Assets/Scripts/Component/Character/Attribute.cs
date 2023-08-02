using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Attribute : MonoBehaviour
{
    private float level = 1;
    private float percent = 0;
    private float amountEXPToLevelUp = 100; 
    private float currentEXP = 0;
    private int currentCoin = 0;

    private float currentHP = 100;
    private float MAX_HP = 100;
    [SerializeField] private RectTransform HPBar;
    [SerializeField] private Ablity ablity;
    [SerializeField] private EntityAnimation _animation;
    private int startCoin = 0;
    private int bonusCoin = 0;

    private DaiBang daibang;
    public void SetDaiBang(DaiBang db)
    {
        daibang = db;
    }
    private void Start()
    {
        startCoin = PlayerPrefs.GetInt(PlayerPrefsString.coinString, 0);
        Observer.Instance.AddObserver(ObserverKey.BonusCoin, OnBonusCoin);
    }
    private void OnDestroy()
    {
        Observer.Instance.RemoveObserver(ObserverKey.BonusCoin, OnBonusCoin);
    }
    private void OnBonusCoin(object data)
    {
        float value = (float)data;
        int valueInt = (int)value;
        bonusCoin += valueInt;
        SaveCoin(startCoin + bonusCoin + currentCoin);
        Observer.Instance.Notify(ObserverKey.Coin, currentCoin);
    }
    public void GetDame(float dame)
    {
        _animation.PlayHitEffect();
        if (currentHP > dame)
        {
            currentHP -= dame;
        }
        else
        {
            currentHP = 0;
        }
        HPBar.sizeDelta = currentHP / MAX_HP * Vector2.right;

        if (currentHP <= 0)
        {
            DucSoundManager.Instance.StopMusic();
            DucSoundManager.Instance.PlaySoundFx("sfx_gameOver");
            Time.timeScale = 0;
            PanelManager.Show<PopUpGameOver>();
        }
    }
    public void OnCollideItem(Item item)
    {
        System.Type type = item.GetType();
        if(type == typeof(EXP))
        {
            DucSoundManager.Instance.PlaySoundFx("sfx_gem");
            OnCollideEXP((EXP)item);
        }
        else if(type == typeof(Coin))
        {
            Coin coin = (Coin)item;
            GameManager.Instance.damageNumber.Spawn((Vector2)coin.transform.position + Vector2.up, coin.coinCount,Color.yellow);
            DucSoundManager.Instance.PlaySoundFx("sfx_coin_double4");
            OnCollideCoin(coin.coinCount);
        }
        else if(type == typeof(Food))
        {
            Food food = (Food)item;
            GameManager.Instance.damageNumber.Spawn((Vector2)transform.position + Vector2.up, food.healh, Color.blue) ;
            OnCollideFood();
        }
        else if(type == typeof(Chest))
        {
            OnCollideChest();
        }
    }
    private void OnCollideChest()
    {
        GameManager.Instance.Pause();
        PanelManager.Show<PopUpChest>();
    }
    private void OnCollideFood() {
        //+50 btw
        daibang.IncreaseHealth(20);
        if (currentHP <= MAX_HP - 20)
        {
            currentHP += 20;
        }
        else
        {
            currentHP = 100;
        }
        HPBar.sizeDelta = currentHP / MAX_HP * Vector2.right;
    }
    private void OnCollideCoin(int coinCount)
    {
        currentCoin+= coinCount;
        SaveCoin(startCoin + bonusCoin + currentCoin);
        Observer.Instance.Notify(ObserverKey.Coin,currentCoin);
    }
    private void SaveCoin(int coin)
    {
        PlayerPrefs.SetInt(PlayerPrefsString.coinString, coin);
    }
    private void OnCollideEXP(EXP exp)
    {
        if(exp.GetEXPType() == EXPType.GREEN)
        {
            currentEXP += 10;
        }
        else if(exp.GetEXPType() == EXPType.BLUE)
        {
            currentEXP += 50;
        }
        else if (exp.GetEXPType() == EXPType.Yellow)
        {
            currentEXP += 250;
        }
        else if (exp.GetEXPType() == EXPType.RED)
        {
            currentEXP += 1250;
        }
        if(currentEXP>= amountEXPToLevelUp)
        {
            currentEXP = 0;
            amountEXPToLevelUp += 100;
            LevelUp();
        }
        SetPercent();
        CanvasManager.Instance.expBarController.SetEXP(percent);
    }
    private void SetPercent()
    {
        percent = (100 * currentEXP / amountEXPToLevelUp);
    }
    private void LevelUp()
    {
        level++;
        amountEXPToLevelUp += 50;
        if (level % 5 == 0){
            MAX_HP += 10;
        };
        SetPercent();
        GameManager.Instance.Pause();
        DucSoundManager.Instance.PlaySoundFx("sfx_autoLevel");
        PopUpReward popUpReward = PanelManager.Show<PopUpReward>();
        popUpReward.ablity = ablity;
        popUpReward.Init();
    }
    
}
