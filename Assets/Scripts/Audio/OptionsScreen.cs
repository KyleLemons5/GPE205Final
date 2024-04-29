using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class OptionsScreen : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider mainVolSlider;
    public Slider SFXVolSlider;
    public Slider musicVolSlider;

    // Start is called before the first frame update
    void Start()
    {
        mainVolSlider.value = 1;
        SFXVolSlider.value = 1;
        musicVolSlider.value = 1;
    }

    // Update is called once per frame
    void Update()
    {
        OnMainVolumeChange();
        OnSFXVolumeChange();
        OnMusicVolumeChange();
    }

    public void OnMainVolumeChange()
    {
        float newVolume = mainVolSlider.value;
        newVolume = Mathf.Log10(newVolume);
        newVolume *= 20;
        audioMixer.SetFloat("MasterVolume", newVolume);
    }

    public void OnSFXVolumeChange()
    {
        float newVolume = mainVolSlider.value;
        newVolume = Mathf.Log10(newVolume);
        newVolume *= 20;
        newVolume += (Mathf.Log10(SFXVolSlider.value) * 20);
        audioMixer.SetFloat("SFXVolume", newVolume);
    }

    public void OnMusicVolumeChange()
    {
        float newVolume = mainVolSlider.value;
        newVolume = Mathf.Log10(newVolume);
        newVolume *= 20;
        newVolume += (Mathf.Log10(musicVolSlider.value) * 20);
        audioMixer.SetFloat("MusicVolume", newVolume);
    }
}
