using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemPowerUp : MonoBehaviour
{
    private int ablityId;
    public TMP_Text ablityName;
    public Image ablityImage;
    public TMP_Text levelText;
    public GameObject selectedObject;
    public Button ItemPowerUpButton;
    private void Start()
    {
        Observer.Instance.AddObserver(ObserverKey.SelectPowerUp, DeActiveSelectObject);
    }
    private void OnDestroy()
    {
        Observer.Instance.RemoveObserver(ObserverKey.SelectPowerUp, DeActiveSelectObject);
    }
    private void DeActiveSelectObject(object data)
    {
        int id = (int)data;
        if (id == ablityId)
        {
            return;
        }
        else if(selectedObject.activeSelf)
        {
            selectedObject.SetActive(false);
        }
    }
    public void InitItem(int id,bool isReInit)
    {
        levelText.text = AblityData.listAb()[id].ablityLevel.ToString();
        if (!isReInit)
        {
            ablityId = id;
            ablityName.text = AblityData.listAb()[id].ablityName;
            ablityImage.sprite = AblityData.listAb()[id].ablitySprite;
            ItemPowerUpButton.onClick.AddListener(OnPowerUpButtonClick);
        }
    }
    private void OnPowerUpButtonClick()
    {
        selectedObject.SetActive(true);
        Observer.Instance.Notify(ObserverKey.SelectPowerUp, ablityId);
    }
}
