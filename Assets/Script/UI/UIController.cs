using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Slider _musicSlider, _sfxSlider;

    private void Start()
    {
        // Save music setting for further use 
        ApplyMusicVolume(); // Start music volume as playerprefs
        ApplySFXVolume(); // Start sfx volume as playerprefs
    }

    public void ToggleMusic()
    {
        AudioManager.Instance.ToggleMusic(); // mute music volume
    }

    public void ToggleSFX()
    {
        AudioManager.Instance.ToggleSFX(); // mute sfx volume
    }

    public void MusicVolume()
    {
        float musicVolume = _musicSlider.value;
        AudioManager.Instance.MusicVolume(musicVolume);
        PlayerPrefs.SetFloat("MusicVolume", musicVolume); // save the updated value to the playerPrefs
    }

    public void SFXVolume()
    {
        float sfxVolume = _sfxSlider.value;
        AudioManager.Instance.SFXVolume(sfxVolume);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume); // save the updated value to the playerPrefs
    }

    private void ApplyMusicVolume()
    {
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.1f); // take music volume from playerprefs if its not exist take default value
        _musicSlider.value = musicVolume; // update slider value
        AudioManager.Instance.MusicVolume(musicVolume); // update MusicVolume on audioManager
    }

    private void ApplySFXVolume()
    {
        float sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 0.2f); // take music volume from playerprefs if its not exist take default value
        _sfxSlider.value = sfxVolume; // update slider value
        AudioManager.Instance.SFXVolume(sfxVolume); // update SFXVolume on audioManager
    }
}