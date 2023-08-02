using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


[UIPanelPrefabAttr("Popup/PopUpLevelSelection", "Canvas")]
public class PopUpLevelSelection : BasePanel
{
    [SerializeField] private TMP_Text coinCount;
    [SerializeField] private Button backButton;

    [SerializeField] private List<ItemLevelSelection> listItems;
    [SerializeField] private SelectLevelSelection selectLevelSelection;

    protected override void Start()
    {
        base.Start();
        backButton.onClick.AddListener(OnBackButtonClick);
        Observer.Instance.AddObserver(ObserverKey.SelectLevel, ActiveSelectedObject);
        Init();
    }
    private void InitCoin()
    {
        int coin = PlayerPrefs.GetInt(PlayerPrefsString.coinString, 0);
        coinCount.text = $"{coin}";
    }
    private void Init()
    {
        InitCoin();
        for (int i = 0; i < listItems.Count; i++)
        {
            listItems[i].InitItem(i);
        }
    }
    private void OnDestroy()
    {
        Observer.Instance.RemoveObserver(ObserverKey.SelectLevel, ActiveSelectedObject);
    }
    private void ActiveSelectedObject(object data)
    {
        int id = (int)data;
        if (!selectLevelSelection.gameObject.activeSelf)
        {
            selectLevelSelection.gameObject.SetActive(true);
        }
        selectLevelSelection.Init(id);
    }
    private void OnBackButtonClick()
    {
        DucSoundManager.Instance.PlaySoundFx("sfx_sounds_pause7_out");
        PanelManager.Show<PopUpCharacterSelection>();
        HidePanel();
    }
}
