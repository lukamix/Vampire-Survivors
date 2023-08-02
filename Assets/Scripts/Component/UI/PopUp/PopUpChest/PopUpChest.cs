using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[UIPanelPrefabAttr("Popup/PopUpChest", "Canvas")]
public class PopUpChest : BasePanel
{
    [SerializeField] private GameObject title;
    [SerializeField] private Button openButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private TMP_Text coinText;
    [SerializeField] private EffectChest effectChest;
    [SerializeField] private Animator chestAnimator;

    private readonly float EFFECT_TIME = 7.5f;
    private float currentTime = 0;

    private bool isWaiting = false;
    private bool isSpinning = false;

    private float rewardCoin;
    private float runningCoin = 0; 
    private readonly int MAX_REWARD_COIN = 100;
    private readonly int MIN_REWARD_COIN = 30;
    private bool isRunningCoin = false;

    protected override void Start()
    {
        base.Start();
        openButton.onClick.AddListener(OnOpenButtonClick);
        closeButton.onClick.AddListener(OnCloseButtonClick);
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        ResetValue();
    }
    private void ResetValue()
    {
        title.SetActive(true);
        openButton.gameObject.SetActive(true);
        closeButton.gameObject.SetActive(false);
        chestAnimator.Play("ChestIdle");
        effectChest.gameObject.SetActive(false);
        coinText.gameObject.SetActive(false);
        isRunningCoin = false;
        runningCoin = 0;
    }
    private void OnOpenButtonClick()
    {
        DucSoundManager.Instance.PauseMusic();
        DucSoundManager.Instance.PlaySoundFx("0213 - Treasure A");
        title.SetActive(false);
        effectChest.gameObject.SetActive(true);
        coinText.gameObject.SetActive(true);
        chestAnimator.Play("OpenChest");
        effectChest.Play();
        RunCoinText();
        isSpinning = true;
        openButton.gameObject.SetActive(false);
    }
    private void RunCoinText()
    {
        rewardCoin = Random.Range(MIN_REWARD_COIN, MAX_REWARD_COIN);
        isRunningCoin = true;
    }
    private void Update()
    {
        if (isSpinning)
        {
            if (!isWaiting)
            {
                StartCoroutine(IEPlay());
            }
        }
        if (isRunningCoin)
        {
            if (runningCoin < rewardCoin)
            {
                runningCoin += Time.unscaledDeltaTime * rewardCoin / EFFECT_TIME;
                coinText.text = runningCoin.ToString();
            }
            else
            {
                coinText.text = rewardCoin.ToString();
            }
        }
    }
    IEnumerator IEPlay()
    {
        isWaiting = true;
        yield return new WaitForSecondsRealtime(0.5f);
        currentTime+=0.5f;
        if (currentTime>= EFFECT_TIME)
        {
            currentTime = 0;
            isSpinning = false;
            isRunningCoin = false;
            coinText.text = rewardCoin.ToString();
            effectChest.OnComplete();
            SaveCoin();
            closeButton.gameObject.SetActive(true);
        }
        isWaiting = false;
    }
    private void SaveCoin()
    {
        Observer.Instance.Notify(ObserverKey.BonusCoin, rewardCoin);
    }
    private void OnCloseButtonClick()
    {
        DucSoundManager.Instance.ContinueMusic();
        closeButton.gameObject.SetActive(false);
        GameManager.Instance.Continue();
        HidePanel();
    }
}
