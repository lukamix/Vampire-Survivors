using Coffee.UIExtensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectChest : MonoBehaviour
{
    [SerializeField] private List<ScrollLineEffect> listScroll;
    [SerializeField] private Button chestButton;

    public void Play()
    {
        int randomValue = Random.Range(1, 4);
        SetActiveScrollLine(randomValue);
        foreach(ScrollLineEffect sle in listScroll)
        {
            if (sle.gameObject.activeSelf)
            {
                sle.Spin();
            }
        }
    }
    public void OnComplete()
    {
        foreach (ScrollLineEffect sle in listScroll)
        {
            if (sle.gameObject.activeSelf)
            {
                sle.StopSpinning();
                int randomValue = Random.Range(0, AblityData.listAb().Count);
                Observer.Instance.Notify(ObserverKey.EquipUpgrade, randomValue);
                sle.SetItem(randomValue);
            }
        }
    } 
    private void SetActiveScrollLine(int number)
    {
        int firstValue;
        if (number == 1)
        {
            firstValue = 1;
        }
        else if(number == 2)
        {
            firstValue = 3;
        }
        else { firstValue = 5; };
        for(int i = 0; i < firstValue; i++)
        {
            listScroll[i].gameObject.SetActive(true);
        }
        for (int i = firstValue; i < listScroll.Count; i++)
        {
            listScroll[i].gameObject.SetActive(false);
        }
    }
}
