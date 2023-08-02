using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemCharacterSelection : MonoBehaviour
{
    [SerializeField] private TMP_Text characterNameText;
    [SerializeField] private Image characterImage;
    [SerializeField] private Image weaponImage;
    [SerializeField] private Button itemCharacteSelectionButton;

    [SerializeField] private GameObject selectedObject;

    private int characterId;
    private void Start()
    {
        Observer.Instance.AddObserver(ObserverKey.SelectCharacter, DeActiveSelectObject);
    }
    private void OnDestroy()
    {
        Observer.Instance.RemoveObserver(ObserverKey.SelectCharacter, DeActiveSelectObject);
    }
    private void DeActiveSelectObject(object data)
    {
        int id = (int)data;
        if (id == characterId)
        {
            return;
        }
        else if (selectedObject.activeSelf)
        {
            selectedObject.SetActive(false);
        }
    }
    public void InitItem(int id)
    {
        characterId = id;
        characterNameText.text = CharacterData.listChar()[id].characterName;
        characterImage.sprite = CharacterData.listChar()[id].characterSprite;
        weaponImage.sprite = CharacterData.listChar()[id].weaponSprite;
        itemCharacteSelectionButton.onClick.AddListener(OnItemCharacteSelectionButtonClick);
    }
    private void OnItemCharacteSelectionButtonClick()
    {
        selectedObject.SetActive(true);
        Observer.Instance.Notify(ObserverKey.SelectCharacter, characterId);
    }
}
