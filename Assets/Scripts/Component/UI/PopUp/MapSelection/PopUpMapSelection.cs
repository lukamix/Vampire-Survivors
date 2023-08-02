using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[UIPanelPrefabAttr("Popup/PopUpMapSelection", "Canvas")]
public class PopUpMapSelection : BasePanel
{
    [SerializeField] private TMP_Text coinCount;
    [SerializeField] private Button backButton;

    [SerializeField] private List<ItemMapSelection> listItems;
    [SerializeField] private SelectedObjectMapSelection selectedObjectMapSelection;

    protected override void Start()
    {
        base.Start();
        backButton.onClick.AddListener(OnBackButtonClick);
        Observer.Instance.AddObserver(ObserverKey.SelectMap, ActiveSelectedObject);
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
        Observer.Instance.RemoveObserver(ObserverKey.SelectMap, ActiveSelectedObject);
    }
    private void ActiveSelectedObject(object data)
    {
        int id = (int)data;
        if (!selectedObjectMapSelection.gameObject.activeSelf)
        {
            selectedObjectMapSelection.gameObject.SetActive(true);
        }
        selectedObjectMapSelection.Init(id);
    }
    private void OnBackButtonClick()
    {
        DucSoundManager.Instance.PlaySoundFx("sfx_sounds_pause7_out");
        PanelManager.Show<PopUpLevelSelection>();
        HidePanel();
    }
}
