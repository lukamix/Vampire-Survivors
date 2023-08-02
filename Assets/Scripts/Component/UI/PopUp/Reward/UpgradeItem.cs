using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeItem : MonoBehaviour
{
    public int upgradeId;
    [SerializeField] private Image upgradeIcon;
    [SerializeField] private TMP_Text upgradeName;
    [SerializeField] private TMP_Text statusText;
    [SerializeField] private TMP_Text upgradeContent;

    private Color passiveColor = new Color(74, 255, 0);
    private Color newColor = new Color(0, 249, 255);
    private Color levelUpColor = new Color(255, 8, 0);
    public void InitItem(int upgradeId,int level)
    {
        this.upgradeId = upgradeId;
        upgradeIcon.sprite = AblityData.listAb()[upgradeId].ablitySprite;
        upgradeName.text = AblityData.listAb()[upgradeId].ablityName;
        upgradeContent.text = AblityData.listAb()[upgradeId].ablityContent;
        if (upgradeId == 0)
        {
            statusText.text = "Passive !!!";
            statusText.color = passiveColor;
        }
        else if(level == 0)
        {
            statusText.text = "New !!!";
            statusText.color = newColor;
        }
        else
        {
            statusText.text = $"+{level}";
            statusText.color = levelUpColor;
        }
    }
    public void EquipUpgrade()
    {
        Observer.Instance.Notify(ObserverKey.EquipUpgrade, upgradeId);
    }
}
