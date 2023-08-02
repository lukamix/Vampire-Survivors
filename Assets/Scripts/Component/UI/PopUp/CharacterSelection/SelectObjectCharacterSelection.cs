using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SelectObjectCharacterSelection : MonoBehaviour
{
    [SerializeField] private TMP_Text characterName;
    [SerializeField] private Image characterSprite;
    [SerializeField] private Image weaponSprite;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private Button confirmButton;
    int selectedID;
    public void Init(int id)
    {
        selectedID = id;
        characterName.text = CharacterData.listChar()[id].characterName;
        characterSprite.sprite = CharacterData.listChar()[id].characterSprite;
        weaponSprite.sprite = CharacterData.listChar()[id].weaponSprite;
        descriptionText.text = CharacterData.listChar()[id].description;
        confirmButton.onClick.AddListener(OnConfirmButtonClick);
    }
    private void OnConfirmButtonClick()
    {
        DucSoundManager.Instance.PlaySoundFx("sfx_sounds_pause7_in");
        PlayerPrefs.SetInt(PlayerPrefsString.characterString, selectedID);
        PanelManager.Show<PopUpLevelSelection>();
        PanelManager.Hide<PopUpCharacterSelection>();
    }
}
