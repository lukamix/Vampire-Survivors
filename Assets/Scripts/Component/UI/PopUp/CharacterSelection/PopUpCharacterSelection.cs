using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[UIPanelPrefabAttr("Popup/PopUpCharacterSelection", "Canvas")]
public class PopUpCharacterSelection : BasePanel
{
    [SerializeField] private TMP_Text coinCount;
    [SerializeField] private Button backButton;

    [SerializeField] private List<ItemCharacterSelection> listItems;
    [SerializeField] private SelectObjectCharacterSelection selectedObject;

    protected override void Start()
    {
        base.Start();
        backButton.onClick.AddListener(OnBackButtonClick);
        Observer.Instance.AddObserver(ObserverKey.SelectCharacter, ActiveSelectedObject);
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
        Observer.Instance.RemoveObserver(ObserverKey.SelectCharacter, ActiveSelectedObject);
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
