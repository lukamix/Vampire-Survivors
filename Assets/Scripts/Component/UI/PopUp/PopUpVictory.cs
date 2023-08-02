using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[UIPanelPrefabAttr("Popup/PopUpVictory", "Canvas")]
public class PopUpVictory : BasePanel
{
    [SerializeField] private Button continueButton;
    protected override void Start()
    {
        base.Start();
        continueButton.onClick.AddListener(OnContinueButtonClick);
    }
    private void OnContinueButtonClick()
    {
        Time.timeScale = 1;
        ScenesManager.Instance.GetScene("StartScene", false);
    }
}
