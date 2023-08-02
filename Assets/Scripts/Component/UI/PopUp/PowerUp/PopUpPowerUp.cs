using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[UIPanelPrefabAttr("Popup/PopUpPowerUp", "Canvas")]
public class PopUpPowerUp : BasePanel
{
    [SerializeField] private TMP_Text coinCount;
    [SerializeField] private Button backButton;
    [SerializeField] private Button refundButton;

    [SerializeField] private List<ItemPowerUp> listItems;
    [SerializeField] private SelectedObjectPowerUp selectedObject;

    protected override void Start()
    {
        base.Start();
        backButton.onClick.AddListener(OnBackButtonClick);
        Observer.Instance.AddObserver(ObserverKey.SelectPowerUp, ActiveSelectedObject);
        Observer.Instance.AddObserver(ObserverKey.PoweredUp, OnPowerUp);
        Init(false);
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        Init(false);
    }
    private void InitCoin()
    {
        int coin = PlayerPrefs.GetInt(PlayerPrefsString.coinString,0);
        coinCount.text = $"{coin}";
    }
    private void Init(bool isReInit)
    {
        InitCoin();
        for (int i = 0; i < listItems.Count; i++)
        {
            listItems[i].InitItem(i, isReInit);
        }
    }
    private void OnDestroy()
    {
        Observer.Instance.RemoveObserver(ObserverKey.SelectPowerUp, ActiveSelectedObject);
        Observer.Instance.RemoveObserver(ObserverKey.PoweredUp, OnPowerUp);
    }
    private void OnPowerUp(object data)
    {
        Init(true);
    }
    private void ActiveSelectedObject(object data)
    {
        int id = (int)data;
        if (!selectedObject.gameObject.activeSelf)
        {
            selectedObject.gameObject.SetActive(true);
        }
        selectedObject.Init(id);
    }
    private void OnBackButtonClick()
    {
        DucSoundManager.Instance.PlaySoundFx("sfx_sounds_pause7_out");
        HidePanel();
    }
}
