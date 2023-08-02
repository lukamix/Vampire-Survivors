using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExpBarController : MonoBehaviour
{
    [SerializeField] private Sprite headFull, headEmpty, tailFull, tailEmpty;
    [SerializeField] private Image head, mid, tail;
    [SerializeField] private TMP_Text percentText;
    private float maxPercent = 1856;
    public void SetEXP(float percent)
    {
        if (percent < 0.5f)
        {
            head.sprite = headEmpty;
            mid.rectTransform.sizeDelta = new Vector2(0, 32);
            tail.sprite = tailEmpty;
        }
        else if (percent < 1.67f)
        {
            head.sprite = headFull;
            mid.rectTransform.sizeDelta = new Vector2(0, 32);
            tail.sprite = tailEmpty;
         }
        else if(percent < 100-1.67f)
        {
            head.sprite = headFull;
            mid.rectTransform.sizeDelta = new Vector2((percent - 1.67f) / (100 - 3.33f) * maxPercent, 32);
            tail.sprite = tailEmpty;
        }
        else
        {
            head.sprite = headFull;
            mid.rectTransform.sizeDelta = new Vector2(maxPercent, 32);
            tail.sprite = tailFull;
        }
        percentText.text = percent + "%";
    }
}
