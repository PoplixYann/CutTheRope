using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionMenu : MonoBehaviour
{
    [SerializeField] GameObject OptionGO;
    [SerializeField] GameObject AreYouSureGO;

    [SerializeField] Slider sfxVolumeSlider;
    [SerializeField] Slider musicVolumeSlider;

    // Start is called before the first frame update
    void Start()
    {
        sfxVolumeSlider.value = 1.0f - (SoundManager.Instance.GetSFXVolume() / -80.0f);
        musicVolumeSlider.value = 1.0f - (SoundManager.Instance.GetMusicVolume() / -80.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnSFXVolumeChanged()
    {
        SoundManager.Instance.SetSFXVolume((1.0f - sfxVolumeSlider.value) * -80.0f);
    }

    public void OnMusicVolumeChanged()
    {
        SoundManager.Instance.SetMusicVolume((1.0f - musicVolumeSlider.value) * -80.0f);
    }

    public void OpenAreYouSure()
    {
        OptionGO.SetActive(false);
        AreYouSureGO.SetActive(true);
    }

    public void CloseAreYouSure()
    {
        OptionGO.SetActive(true);
        AreYouSureGO.SetActive(false);
    }

    public void ResetSave()
    {
        GameManager.Instance.DeleteLevelsSave();
    }
}
