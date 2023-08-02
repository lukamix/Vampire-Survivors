using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartSceneManager : MonoBehaviour
{
    [SerializeField] private Button tapToStartButton;
    [SerializeField] private GameObject listButton;

    [SerializeField] private Button startButton;
    [SerializeField] private Button powerUpButton;
    [SerializeField] private Button settingsButton;

    private void Start()
    {
        tapToStartButton.onClick.AddListener(OnTapToStart);
        startButton.onClick.AddListener(OnStartButtonClick);
        powerUpButton.onClick.AddListener(OnPowerUpButtonClick);
        settingsButton.onClick.AddListener(OnSettingButtonClick);
        CheckActive(listButton);
        DucSoundManager.Instance.PlaySoundFx("0101 - Side A");
    }
    private void CheckActive(GameObject go)
    {
        if (go != null && go.activeSelf)
        {
            go.SetActive(false);
        }
    }
    private void OnTapToStart()
    {
        DucSoundManager.Instance.PlaySoundFx("sfx_sounds_pause7_in");
        tapToStartButton.gameObject.SetActive(false);
        listButton.SetActive(true);
    }
    private void OnStartButtonClick()
    {
        DucSoundManager.Instance.PlaySoundFx("sfx_sounds_pause7_in");
        PanelManager.Show<PopUpCharacterSelection>();
    }
    private void OnPowerUpButtonClick()
    {
        DucSoundManager.Instance.PlaySoundFx("sfx_sounds_pause7_in");
        PanelManager.Show<PopUpPowerUp>();
    }
    private void OnSettingButtonClick()
    {
        DucSoundManager.Instance.PlaySoundFx("sfx_sounds_pause7_in");
        PopUpSettings ps = PanelManager.Show<PopUpSettings>();
        ps.ActiveQuitButtonForIngame(false);
    }
}
