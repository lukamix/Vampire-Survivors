using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemMapSelection : MonoBehaviour
{
    [SerializeField] private Button mapItemButton;
    [SerializeField] private GameObject selectedObject;
    private int id;
    private void Start()
    {
        mapItemButton.onClick.AddListener(OnMapItemButtonClick);
    }
    public void InitItem(int id)
    {
        this.id = id;
    }
    private void OnMapItemButtonClick()
    {
        selectedObject.SetActive(true);
        Observer.Instance.Notify(ObserverKey.SelectMap, id);
    }
}
