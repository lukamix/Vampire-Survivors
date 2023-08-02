using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[UIPanelPrefabAttr("Popup/PopupReward","Canvas")]
public class PopUpReward : BasePanel
{
    [SerializeField] private List<UpgradeItem> upgradeItemList;
    protected override void Start()
    {
        base.Start();
        Observer.Instance.AddObserver(ObserverKey.EquipUpgrade, OnEquipItem);
    }
    private void OnDestroy()
    {
        Observer.Instance.RemoveObserver(ObserverKey.EquipUpgrade, OnEquipItem);
    }
    [HideInInspector] public Ablity ablity;
    public void Init()
    {
        List<int> listAblityId = new List<int>();
        for (int i = 0; i < AblityData.listAb().Count; i++)
        {
            listAblityId.Add(i);
        }
        for (int i = 0; i < upgradeItemList.Count; i++)
        {
            int randomValue = Random.Range(0, listAblityId.Count);
            int randomAblityId = listAblityId[randomValue];
            upgradeItemList[i].InitItem(randomAblityId, ablity.GetAblityLevel(randomAblityId));
            listAblityId.Remove(randomAblityId);
        }
    }
    private void OnEquipItem(object data)
    {
        if (gameObject.activeSelf)
        {
            GameManager.Instance.Continue();
            HidePanel();
        }
    }
}
