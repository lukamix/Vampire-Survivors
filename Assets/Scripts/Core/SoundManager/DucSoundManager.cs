﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class DucSoundManager : DucSingleton<DucSoundManager>
{

    private static float _soundFXVolume = 1.0f;
    public static float soundFXVolume
    {
        get
        {
            return _soundFXVolume;
        }
        set
        {
            _soundFXVolume = value;
            PlayerPrefs.SetFloat("soundFX_volume", value);
        }
    }
    private static float _videoVolume = 1.0f;
    public static float videoVolume
    {
        get
        {
            return _videoVolume;
        }
        set
        {
            _videoVolume = value;
            PlayerPrefs.SetFloat("video_volume", value);
        }
    }
    private static float _musicVolume = 1.0f;
    public static float musicVolume
    {
        get
        {
            return _musicVolume;
        }
        set
        {
            _musicVolume = value;
            PlayerPrefs.SetFloat("music_volume", value);
        }
    }


    private static bool is_music_muted = false;
    private static bool isMusicMute
    {
        get
        {
            return is_music_muted;
        }
        set
        {
            PlayerPrefs.SetFloat("is_music_muted", value ? 1 : 0);
            is_music_muted = value;
        }
    }
    private static bool is_fx_muted = false;
    private static bool isFxMuted
    {
        get
        {
            return is_fx_muted;
        }
        set
        {
            PlayerPrefs.SetFloat("is_fx_muted", value ? 1 : 0);
            is_fx_muted = value;
        }
    }

    private float fromLerp;
    private float toLerp;
    private float lerpDuration;
    private float startLerp;

    [SerializeField]
    private AudioClip[] audioClips;

    private Dictionary<string, AudioClip> dictAudios = new Dictionary<string, AudioClip>();

    [SerializeField]
    private AudioSource default_audio_source_fx, default_audio_source_music;
    private AudioSource defaultAudioSourceFx => default_audio_source_fx;
    private AudioSource defaultAudioSourceMusic => default_audio_source_music;


    private void Update()
    {
        if (lerpDuration > 0)
        {
            float percent = (Time.time - startLerp) / lerpDuration;

            if (percent > 1.0f)
                percent = 1.0f;

            float volume = fromLerp + (toLerp - fromLerp) * percent;

            if (defaultAudioSourceFx != null)
            {
                defaultAudioSourceFx.volume = volume;
            }

            if (volume >= toLerp) //stop lerp audio volume
            {
                lerpDuration = 0;
            }
        }

    }

    protected override void Awake()
    {
        dontDestroyOnLoad = true;
        base.Awake();
    }

    private void Start()
    {
        soundFXVolume = PlayerPrefs.GetFloat("soundFX_volume", 1);
        musicVolume = PlayerPrefs.GetFloat("music_volume", 1);
        videoVolume = PlayerPrefs.GetFloat("video_volume", 1);
        is_music_muted = PlayerPrefs.GetInt("is_music_muted", 0) == 1 ? true : false;
        is_fx_muted = PlayerPrefs.GetInt("is_fx_muted", 0) == 1 ? true : false;
        SetMusicMute(is_music_muted);
        AddAudioToDict();
    }

    private void AddAudioToDict()
    {
        if (audioClips == null || audioClips.Length == 0) return;
        int max = audioClips.Length;
        for (int i = 0; i < max; i++)
        {
            AudioClip clip = audioClips[i];
            if (!dictAudios.ContainsKey(clip.name))
                dictAudios.Add(clip.name, clip);
        }
    }

    public void SetSoundFxVolume(float volume)
    {
        soundFXVolume = volume;
        if (defaultAudioSourceFx != null)
            defaultAudioSourceFx.volume = volume;
    }

    public void SetMusicVolme(float volume)
    {
        musicVolume = volume;
        if (defaultAudioSourceMusic != null)
            defaultAudioSourceMusic.volume = volume;
    }

    private AudioClip GetAudio(string audio_name)
    {
        if (string.IsNullOrEmpty(audio_name)) return null;
        if (dictAudios.ContainsKey(audio_name))
        {
            return dictAudios[audio_name];
        }
        if (audioClips == null || audioClips.Length == 0) return null;
        int max = audioClips.Length;
        for (int i = 0; i < max; i++)
        {
            if (audioClips[i] == null) return null;
            AudioClip clip = audioClips[i];
            if (clip.name.Equals(audio_name))
            {
                if (!dictAudios.ContainsKey(audio_name))
                    dictAudios.Add(clip.name, clip);
                return clip;
            }
        }
        return null;
    }

    public void PlaySoundFxOnGameObject(GameObject gameObj, AudioClip audioClip, bool isLoop = false)
    {
        if (gameObj == null) return;
        AudioSource audioSource = gameObj.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObj.AddComponent<AudioSource>();
        }
        audioSource.spatialBlend = 1f;
        audioSource.minDistance = 1;
        audioSource.maxDistance = 15;
        PlaySound(audioSource, audioClip, isLoop);
    }
    public void PlaySoundFxOnGameObject(GameObject gameObj, string sound_name, bool isLoop = false)
    {
        if (gameObj == null) return;
        AudioSource audioSource = gameObj.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObj.AddComponent<AudioSource>();
        }
        audioSource.spatialBlend = 1f;
        audioSource.minDistance = 1;
        audioSource.maxDistance = 10;
        AudioClip audio = GetAudio(sound_name);

        PlaySound(audioSource, audio, isLoop);
    }

    public void PlaySoundFx(AudioSource audio_source, AudioClip audio, float volume = 1)
    {
        PlaySound(audio_source, audio, volume);
    }

    public void PlaySoundFx(AudioSource audio_source, string soundfx, float volume = 1)
    {
        AudioClip audio = GetAudio(soundfx);
        PlaySound(audio_source, audio, volume);
    }

    public void PlaySoundFx(string soundfx, float volume)
    {
        AudioClip audio = GetAudio(soundfx);

        PlaySound(audio, volume * soundFXVolume);
    }

    public void PlaySoundFx(string soundfx, bool isLoop = false)
    {
        AudioClip audio = GetAudio(soundfx);

        PlaySound(audio, isLoop);
    }

    private void PlaySound(AudioSource audio_source, AudioClip soundClip, bool isLoop = false)
    {
        if (audio_source == null || soundClip == null) return;
        audio_source.mute = isFxMuted;
        audio_source.loop = isLoop;
        audio_source.PlayOneShot(soundClip, soundFXVolume);
    }

    private void PlaySound(AudioClip soundClip, bool isLoop = false)
    {
        if (defaultAudioSourceFx == null || soundClip == null) return;
        defaultAudioSourceFx.mute = isFxMuted;
        defaultAudioSourceFx.loop = isLoop;
        defaultAudioSourceFx.PlayOneShot(soundClip, soundFXVolume);
    }

    private void PlaySound(AudioSource audio_source, AudioClip audioClip, float volume)
    {
        if (audio_source == null || audioClip == null) return;
        audio_source.mute = isFxMuted;
        audio_source.volume = volume;
        audio_source.PlayOneShot(audioClip, volume);
    }

    private void PlaySound(AudioClip audioClip, float volume)
    {
        if (defaultAudioSourceFx == null || audioClip == null)
            return;
        defaultAudioSourceFx.mute = isFxMuted;
        defaultAudioSourceFx.volume = volume;
        defaultAudioSourceFx.PlayOneShot(audioClip, volume);
    }

    public void PlaySoundFx(bool isLoop = false)
    {

        if (defaultAudioSourceFx == null)
            return;
        defaultAudioSourceFx.loop = isLoop;
        defaultAudioSourceFx.mute = isFxMuted;
        //soundFx.tag = "soundfx";
        defaultAudioSourceFx.volume = soundFXVolume;
        defaultAudioSourceFx.Play();

    }

    public void StopAllSoundFx(GameObject gameObject)
    {
        if (gameObject == null)
        {
            if (defaultAudioSourceFx != null)
                defaultAudioSourceFx.Stop();
        }
        else
        {
            AudioSource source = gameObject.GetComponent<AudioSource>();
            if (source != null)
                source.Stop();
        }
    }




    public void PlayMusic(AudioClip audioClip, bool isLoop)
    {
        if (defaultAudioSourceMusic == null)
            return;
        defaultAudioSourceMusic.clip = audioClip;
        defaultAudioSourceMusic.mute = isMusicMute;
        //audio.tag = "music";
        defaultAudioSourceMusic.volume = musicVolume;
        defaultAudioSourceMusic.loop = isLoop;
        defaultAudioSourceMusic.Play();
    }

    public void PlayMusic(AudioSource audio, bool isLoop = true)
    {
        if (audio == null)
            return;
        audio.mute = isMusicMute;
        //audio.tag = "music";
        audio.volume = musicVolume;
        audio.loop = isLoop;
        audio.Play();
    }

    public void PlayMusic(string audioName, bool isLoop = true, float minLerpVolume = 1.0f, float maxLerpVolume = 1.0f, float lerpDuration = 0.0f)
    {
        StopAudioMusic();
        AudioClip clip = GetAudio(audioName);
        if (clip == null) return;
        fromLerp = minLerpVolume;
        toLerp = maxLerpVolume;
        this.lerpDuration = lerpDuration;
        this.startLerp = Time.time;
        defaultAudioSourceMusic.mute = isMusicMute;
        defaultAudioSourceMusic.clip = clip;
        defaultAudioSourceMusic.loop = isLoop;
        defaultAudioSourceMusic.volume = musicVolume;
        defaultAudioSourceMusic.Play();
    }
    public void StopMusic()
    {
        StopAudioFx();
        StopAudioMusic();
    }
    public void PauseMusic()
    {
        if (defaultAudioSourceMusic != null)
        {
            defaultAudioSourceMusic.Pause();
        }
    }
    public void ContinueMusic()
    {
        if (defaultAudioSourceMusic != null)
        {
            defaultAudioSourceMusic.UnPause();
        }
    }

    public void StopAudioFx()
    {
        if (defaultAudioSourceFx != null)
        {
            defaultAudioSourceFx.Stop();
        }
    }

    public void StopAudioMusic()
    {
        if (defaultAudioSourceMusic != null)
        {
            defaultAudioSourceMusic.Stop();
        }
    }

    public void SetMute(bool isMute)
    {
        SetSoundMute(isMute);
        SetMusicMute(isMute);
    }

    public void SetSoundMute(bool isMuted)
    {
        isFxMuted = isMuted;
        if (defaultAudioSourceFx != null)
        {
            defaultAudioSourceFx.mute = isMuted;
        }

    }


    public void SetMusicMute(bool isMuted)
    {
        isMusicMute = isMuted;
        if (defaultAudioSourceMusic != null)
        {
            defaultAudioSourceMusic.mute = isMuted;
        }
    }
    private Dictionary<string, AudioSource> dic_AudioSource = new Dictionary<string, AudioSource>();

    public void RegisterVideoSound(string key, AudioSource audio_value)
    {
        if (!string.IsNullOrEmpty(key) && audio_value != null)
            if (!dic_AudioSource.ContainsKey(key))
            {
                dic_AudioSource.Add(key, audio_value);
            }
    }
    public void SetVolumeVideo(float volumn)
    {
        videoVolume = volumn;
        if (dic_AudioSource == null) return;
        foreach (var item in dic_AudioSource)
        {
            if(item.Value == null)
            {
                dic_AudioSource.Remove(item.Key);
                continue;
            }
            item.Value.volume = volumn;
        }
    }
    public bool GetStatusAudioSource(string key)
    {
        if (!String.IsNullOrEmpty(key) && dic_AudioSource.ContainsKey(key))
        {
            if (dic_AudioSource[key] != null)
                return dic_AudioSource[key].mute;
            else
            {
                dic_AudioSource.Remove(key);
                return false;
            }
        }
        return true;
    }

    public void MuteSoundVideo(string key, bool is_mute)
    {
        if (dic_AudioSource.ContainsKey(key) && !string.IsNullOrEmpty(key))
        {
            if (dic_AudioSource[key] != null)
                dic_AudioSource[key].mute = is_mute;
            else
                dic_AudioSource.Remove(key);
        }
    }

    public void ClearAllSoundOfVideo()
    {
        dic_AudioSource.Clear();
    }

    private Dictionary<string, AudioSource> dict_AudioSourceSFX = new Dictionary<string, AudioSource>();
    private bool _checkAdd = false;
    private void AddDefaultSFX()
    {
        if (_checkAdd == false)
        {
            dict_AudioSourceSFX.Add("default", default_audio_source_fx);
            _checkAdd = true;
        }
    }
    public void RegisterSFX(string key, AudioSource audio_value)
    {
        AddDefaultSFX();
        if (!string.IsNullOrEmpty(key) && audio_value != null)
            if (!dict_AudioSourceSFX.ContainsKey(key))
            {
                dict_AudioSourceSFX.Add(key, audio_value);
            }
    }
    public void PlaySFX(string key, string soundfx, float volume = 1, bool loop = true)
    {
        AudioClip audio = GetAudio(soundfx);
        if (dict_AudioSourceSFX.ContainsKey(key) && !String.IsNullOrEmpty(key))
        {
            AudioSource audio_source = dict_AudioSourceSFX[key];
            if (audio_source == null || audio == null) return;
            audio_source.mute = false;
            audio_source.volume = volume;
            audio_source.Play();
            audio_source.loop = loop;
        }

    }
    public void SetVolumeSFX(float volume)
    {
        soundFXVolume = volume;
        if (dict_AudioSourceSFX == null) return;
        foreach (var item in dict_AudioSourceSFX)
        {
            item.Value.volume = volume;
        }
    }
    public void MuteSFX(string key, bool is_mute)
    {
        if (dict_AudioSourceSFX.ContainsKey(key) && !String.IsNullOrEmpty(key))
        {
            dict_AudioSourceSFX[key].mute = is_mute;
        }
    }
    public void MuteSFX(bool is_mute)
    {
        if (dict_AudioSourceSFX == null) return;
        foreach (var item in dict_AudioSourceSFX)
        {
            item.Value.mute = is_mute;
        }

    }
    public void ClearAllSFX()
    {
        dict_AudioSourceSFX.Clear();
    }

}
