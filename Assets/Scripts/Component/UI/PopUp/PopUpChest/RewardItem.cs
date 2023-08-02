using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardItem : MonoBehaviour
{
    [SerializeField] private Image rewardItemImage;

    public void SetItem(int id)
    {
        rewardItemImage.sprite = AblityData.listAb()[id].ablitySprite;
    }
}
