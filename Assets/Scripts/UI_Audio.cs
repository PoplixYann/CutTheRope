using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class UI_Audio : MonoBehaviour
{
    static UI_Audio instance;
    public static UI_Audio Instance
    {
        get
        {
            return instance;
        }
    }

    AudioSource clickSource;
    [SerializeField] AudioClip clickClip;
    [SerializeField] AudioMixerGroup sfxMixer;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        clickSource = gameObject.AddComponent<AudioSource>();
        clickSource.clip = clickClip;
        clickSource.outputAudioMixerGroup = sfxMixer;
    }

    
    void s_PlayClick()
    {
        clickSource.Play();
    }

    public static void PlayClick()
    {
        Instance.s_PlayClick();
    }
}
