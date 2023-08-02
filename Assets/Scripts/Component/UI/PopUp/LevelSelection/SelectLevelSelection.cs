using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectLevelSelection : MonoBehaviour
{
    [SerializeField] private TMP_Text levelNameText;
    [SerializeField] private TMP_Text levelDescriptionText;
    [SerializeField] private Button confirmButton;
    int id;

    public void Init(int id)
    {
        this.id = id;
        levelNameText.text = LevelData.levelData()[id,0];
        levelDescriptionText.text= LevelData.levelData()[id, 1];
        confirmButton.onClick.AddListener(OnConfirmButtonClick);
    }
    private void OnConfirmButtonClick()
    {
        DucSoundManager.Instance.PlaySoundFx("sfx_sounds_pause7_in");
        PlayerPrefs.SetInt(PlayerPrefsString.levelString, id);
        PanelManager.Show<PopUpMapSelection>();
        PanelManager.Hide<PopUpLevelSelection>();
    }
}
