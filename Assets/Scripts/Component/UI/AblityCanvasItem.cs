using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AblityCanvasItem : MonoBehaviour
{
    [SerializeField] private Image ablityImage;
    [SerializeField] private TMP_Text ablityLevelText;
    private int ablityId;
    private int ablityLevel;

    public void InitAblity(int ablity_id)
    {
        ablityId = ablity_id;
        ablityImage.sprite = AblityData.listAb()[ablity_id].ablitySprite;
        ablityLevel = AblityData.listAb()[ablity_id].ablityLevel;
        if(ablityLevel == 0)
        {
            ablityLevel = 1;
        }
        ablityLevelText.text = $"{ablityLevel}";
    }
    public void IncreaseAblityLevel()
    {
        ablityLevel++;
        ablityLevelText.text = $"{ablityLevel}";
    }
    public int GetAblityId()
    {
        return ablityId;
    }
}
