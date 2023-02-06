using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_OptionInGame : MonoBehaviour
{
    [SerializeField] Slider sfxVolumeSlider;
    [SerializeField] Slider musicVolumeSlider;

    // Start is called before the first frame update
    void Start()
    {
        sfxVolumeSlider.value = 1.0f - (SoundManager.Instance.GetSFXVolume() / -80.0f);
        musicVolumeSlider.value = 1.0f - (SoundManager.Instance.GetMusicVolume() / -80.0f);
    }

    public void OnSFXVolumeChanged()
    {
        SoundManager.Instance.SetSFXVolume((1.0f - sfxVolumeSlider.value) * -80.0f);
    }

    public void OnMusicVolumeChanged()
    {
        SoundManager.Instance.SetMusicVolume((1.0f - musicVolumeSlider.value) * -80.0f);
    }
}
