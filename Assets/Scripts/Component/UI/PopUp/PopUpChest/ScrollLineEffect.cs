using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollLineEffect : MonoBehaviour
{
    [SerializeField] private List<RectTransform> listItem;
    [SerializeField] private RewardItem rewardItem;

    private readonly int LENGTH = 315;

    private readonly int speed = 2;

    private bool isSpinning;

    private void Start()
    {
        MixList();
    }
    private void Update()
    {
        if (isSpinning)
        {
            for (int i = 0; i < listItem.Count; i++)
            {
                if (listItem[i].anchoredPosition.y >= LENGTH)
                {
                    listItem[i].anchoredPosition = new Vector2(0, -LENGTH);
                }
                listItem[i].anchoredPosition = new Vector2(0, listItem[i].anchoredPosition.y + speed);
            }
        }
    }
    public void Spin()
    {
        isSpinning = true;
        rewardItem.gameObject.SetActive(false);
        SetActiveItems(true);
    }
    public void StopSpinning()
    {
        isSpinning = false;
        SetActiveItems(false);
    }
    public void SetItem(int id)
    {
        rewardItem.SetItem(id);
        rewardItem.gameObject.SetActive(true);
    }
    private void SetActiveItems(bool active)
    {
        for(int i = 0; i < listItem.Count; i++)
        {
            listItem[i].gameObject.SetActive(active);
        }
    }
    private void MixList()
    {
        List<int> list1 = new List<int>();
        for(int i = 0; i < listItem.Count; i++)
        {
            list1.Add(i);
        }
        List<RectTransform> newList = new List<RectTransform>();
        for (int i = 0; i < listItem.Count; i++)
        {
            int randomValue = Random.Range(0, list1.Count);
            newList.Add(listItem[list1[randomValue]]);
            list1.Remove(list1[randomValue]);
        }
        listItem.Clear();
        for(int i = 0; i < newList.Count; i++)
        {
            listItem.Add(newList[i]);
        }
    }
}
