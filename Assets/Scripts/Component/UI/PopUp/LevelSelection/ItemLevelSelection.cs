using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemLevelSelection : MonoBehaviour
{
    [SerializeField] private Button levelSelectButton;
    [SerializeField] private GameObject selectedObject;
    int id;
    private void Start()
    {
        Observer.Instance.AddObserver(ObserverKey.SelectLevel, DeActiveSelectObject);
    }
    private void OnDestroy()
    {
        Observer.Instance.RemoveObserver(ObserverKey.SelectLevel, DeActiveSelectObject);
    }
    private void DeActiveSelectObject(object data)
    {
        int id = (int)data;
        if (this.id == id)
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
        this.id = id;
        levelSelectButton.onClick.AddListener(OnLevelSelectButtonClick);
    }
    private void OnLevelSelectButtonClick()
    {
        selectedObject.SetActive(true);
        Observer.Instance.Notify(ObserverKey.SelectLevel, id);
    }
}