using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[UIPanelPrefabAttr("Popup/PopupSettings", "Canvas")]
public class PopUpSettings : BasePanel
{
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    [SerializeField] private TMP_Text musicText;
    [SerializeField] private TMP_Text sfxText;

    [SerializeField] private Button backButton;
    [SerializeField] private Button oKButton;
    [SerializeField] private Button quitButton;
    private float startMusicVolume;
    private float startSfxVolume;

    private bool ingame = false;
    protected override void Start()
    {
        base.Start();
        musicSlider.onValueChanged.AddListener(delegate { OnMusicSliderValueChange(); });
        sfxSlider.onValueChanged.AddListener(delegate { OnSfxSliderValueChange(); });
        backButton.onClick.AddListener(ClosePanel);
        oKButton.onClick.AddListener(ClosePanel);
        quitButton.onClick.AddListener(LoadStartScene);

        startMusicVolume = DucSoundManager.musicVolume;
        musicSlider.value = startMusicVolume;

        int valueMusicInt = (int)(startMusicVolume * 100);
        musicText.text = valueMusicInt.ToString();

        startSfxVolume = DucSoundManager.soundFXVolume;
        sfxSlider.value = startSfxVolume;
        int valueSfxInt = (int)(startSfxVolume * 100);
        sfxText.text = valueSfxInt.ToString();

    }
    public void ActiveQuitButtonForIngame(bool active)
    {
        quitButton.gameObject.SetActive(active);
        ingame = active;
    }
    private void OnMusicSliderValueChange()
    {
        float value = musicSlider.value;
        int valueInt = (int)(value*100);
        musicText.text = valueInt.ToString();
        DucSoundManager.Instance.SetMusicVolme(value);
    }
    private void OnSfxSliderValueChange()
    {
        float value = sfxSlider.value;
        int valueInt = (int)(value*100);
        sfxText.text = valueInt.ToString();
        DucSoundManager.Instance.SetVolumeSFX(value);
    }
    private void LoadStartScene()
    {
        GameManager.Instance.Continue();
        DucSoundManager.Instance.StopMusic();
        ScenesManager.Instance.GetScene("StartScene", false);
    }
    private void ClosePanel()
    {
        if (ingame)
        {
            GameManager.Instance.Continue();
        }
        DucSoundManager.Instance.PlaySoundFx("sfx_sounds_pause7_out");
        HidePanel();
    }
}
