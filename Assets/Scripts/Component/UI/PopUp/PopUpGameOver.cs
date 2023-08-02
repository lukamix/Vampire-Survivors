using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[UIPanelPrefabAttr("Popup/PopupGameOver", "Canvas")]
public class PopUpGameOver : BasePanel
{
    [SerializeField] private Button continueButton;
    protected override void Start()
    {
        base.Start();
        continueButton.onClick.AddListener(OnContinueButtonClick);
        int currentCoin = PlayerPrefs.GetInt(PlayerPrefsString.coinString, 0);
        PlayerPrefs.SetInt(PlayerPrefsString.coinString, currentCoin + 500);
    }
    private void OnContinueButtonClick()
    {
        Time.timeScale = 1;
        ScenesManager.Instance.GetScene("StartScene", false);
    }
}
