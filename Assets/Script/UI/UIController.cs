using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Slider _musicSlider, _sfxSlider;

    private void Start()
    {
        // PlayerPrefs'ten kayýtlý müzik ve ses efekti seviyelerini kontrol edin
        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            float musicVolume = PlayerPrefs.GetFloat("MusicVolume");
            _musicSlider.value = musicVolume;
            AudioManager.Instance.MusicVolume(musicVolume);
        }

        if (PlayerPrefs.HasKey("SFXVolume"))
        {
            float sfxVolume = PlayerPrefs.GetFloat("SFXVolume");
            _sfxSlider.value = sfxVolume;
            AudioManager.Instance.SFXVolume(sfxVolume);
        }
    }

    public void ToggleMusic()
    {
        AudioManager.Instance.ToggleMusic();
    }

    public void ToggleSFX()
    {
        AudioManager.Instance.ToggleSFX();
    }

    public void MusicVolume()
    {
        float musicVolume = _musicSlider.value;
        PlayerPrefs.SetFloat("MusicVolume", musicVolume); // Müzik seviyesini PlayerPrefs'e kaydet
        AudioManager.Instance.MusicVolume(musicVolume);
    }

    public void SFXVolume()
    {
        float sfxVolume = _sfxSlider.value;
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume); // SFX seviyesini PlayerPrefs'e kaydet
        AudioManager.Instance.SFXVolume(sfxVolume);
    }
}
