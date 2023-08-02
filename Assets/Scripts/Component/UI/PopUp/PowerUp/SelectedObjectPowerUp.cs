using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SelectedObjectPowerUp : MonoBehaviour
{
    [SerializeField] private TMP_Text ablityName;
    [SerializeField] private Image ablitySprite;
    [SerializeField] private TMP_Text ablityContent;
    [SerializeField] private TMP_Text costText;
    [SerializeField] private Button buyButton;
    [SerializeField] private GameObject notEnoughMoneyObject;
    int selectedID;
    int cointToBuy;
    bool isReInit =false;
    public void Init(int id)
    {
        selectedID = id;
        ablityName.text = AblityData.listAb()[id].ablityName;
        ablitySprite.sprite = AblityData.listAb()[id].ablitySprite;
        ablityContent.text = AblityData.listAb()[id].ablityContent;
        SetCoinToBuy(id);
        if (!isReInit)
        {
            buyButton.onClick.AddListener(OnBuyButtonClick);
        }
        CheckMoney();
        isReInit = true;
    }
    private void SetCoinToBuy(int id)
    {
        cointToBuy = (AblityData.listAb()[id].ablityLevel + 1) * 100;
        costText.text = $"{cointToBuy}";
    }
    private void CheckMoney()
    {
        SetCoinToBuy(selectedID);
        int money = PlayerPrefs.GetInt(PlayerPrefsString.coinString, 0);
        if(money< cointToBuy)
        {
            buyButton.gameObject.SetActive(false);
            notEnoughMoneyObject.SetActive(true);
        }
        else
        {
            buyButton.gameObject.SetActive(true);
            notEnoughMoneyObject.SetActive(false);
        }
    }
    private void UpgradeAblity()
    {
        int money = PlayerPrefs.GetInt(PlayerPrefsString.coinString, 0);
        PlayerPrefs.SetInt(PlayerPrefsString.coinString, money - cointToBuy);
        AblityData.listAb()[selectedID].UpgradeAb();
        Observer.Instance.Notify(ObserverKey.PoweredUp);
    }
    private void OnBuyButtonClick()
    {
        DucSoundManager.Instance.PlaySoundFx("sfx_sounds_pause7_in");
        UpgradeAblity();
        CheckMoney();
    }
}
