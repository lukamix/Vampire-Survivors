using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SelectedObjectMapSelection : MonoBehaviour
{
    [SerializeField] private TMP_Text mapNameText;
    [SerializeField] private Button startButton;
    int selectedID;
    public void Init(int id)
    {
        selectedID = id;
        mapNameText.text = "Mad Forest";
        startButton.onClick.AddListener(OnStartButtonClick);
    }
    private void OnStartButtonClick()
    {
        DucSoundManager.Instance.PlaySoundFx("sfx_sounds_pause7_in");
        ScenesManager.Instance.GetScene("Splash", false);
    }
}
