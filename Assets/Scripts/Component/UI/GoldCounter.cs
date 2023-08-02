using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GoldCounter : MonoBehaviour
{
    [SerializeField] private TMP_Text goldText;
    private void Start()
    {
        Observer.Instance.AddObserver(ObserverKey.Coin, SetGoldText);
    }
    private void OnDestroy()
    {
        Observer.Instance.RemoveObserver(ObserverKey.Coin, SetGoldText);
    }
    public void SetGoldText(object data)
    {
        int gold_count = (int)data;
        goldText.text = $"{gold_count}";
    }
}
