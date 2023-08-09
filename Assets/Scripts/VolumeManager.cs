using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeManager : MonoBehaviour
{
    private static VolumeManager instance;
    public static VolumeManager Instance
    {
        get { return instance; }
    }
    public static float sfxVolume { get; private set; }
    public static float bgmVolume { get; private set; }

    [SerializeField] private AudioMixer mixer;

    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider bgmSlider;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }
    public void UpdateSFXVolume()
    {
        sfxVolume = sfxSlider.value;

        if (sfxVolume == 0)
        {
            sfxVolume = 0.001f;
        }
        UpdateMixer();
    }
    public void UpdateBGMVolume()
    {
        bgmVolume = bgmSlider.value;
        if (bgmVolume == 0)
        {
            bgmVolume = 0.001f;
        }
        UpdateMixer();
    }
    private void UpdateMixer()
    {
        mixer.SetFloat("sfxVolume", Mathf.Log10(sfxVolume) * 20);
        mixer.SetFloat("bgmVolume", Mathf.Log10(bgmVolume) * 20);
    }
    public void UpdateSliders()
    {
        sfxSlider.value = sfxVolume;
        bgmSlider.value = bgmVolume;
    }
    public void SetVolumes(float newSFXVolume, float newBGMVolume)
    {
        sfxVolume = newSFXVolume;
        bgmVolume = newBGMVolume;

        UpdateSliders();
        UpdateMixer();
    }
}
