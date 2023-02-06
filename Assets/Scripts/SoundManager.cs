using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System.IO;

public class SoundManager : MonoBehaviour
{
    static SoundManager instance;
    public static SoundManager Instance
    {
        get
        {
            return instance;
        }
    }

    List<AudioSource> audioSourceList = new List<AudioSource>();

    [SerializeField] AudioMixerGroup sfxMixer;
    [SerializeField] AudioMixerGroup musicMixer;
    [SerializeField] AudioMixerSnapshot pauseSnapshot;
    [SerializeField] AudioMixerSnapshot unpauseSnapshot;

    string audioSavePath;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this);

        audioSavePath = Path.Combine(Application.persistentDataPath, "audio.txt");

    }

    private void Start()
    {
        LoadAudioOption();
    }

    private void Update()
    {
        for (int i = 0; i < audioSourceList.Count; i++)
        {
            if (!audioSourceList[i].isPlaying)
            {
                Destroy(audioSourceList[i]);
                audioSourceList.RemoveAt(i);
                i--;
            }
        }
    }

    public bool MusicIsPlaying()
    {
        bool isMusicPlaying = false;

        for (int i = 0; i < audioSourceList.Count; i++)
        {
            if (!audioSourceList[i].outputAudioMixerGroup == musicMixer)
            {
                isMusicPlaying = true;
            }
        }

        return isMusicPlaying;
    }

    public void PlaySound(Sound sound)
    {
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = sound.clip;

        if (sound.type == SoundType.SFX)
        {
            audioSource.outputAudioMixerGroup = sfxMixer;
        }
        else if (sound.type == SoundType.MUSIC)
        {
            audioSource.outputAudioMixerGroup = musicMixer;
            audioSource.loop = true;
            for (int i = 0; i < audioSourceList.Count; i++)
            {
                if (audioSourceList[i].outputAudioMixerGroup == musicMixer)
                {
                    if (audioSourceList[i].clip == sound.clip)
                    {
                        Destroy(audioSource);
                        return;
                    }

                    Destroy(audioSourceList[i]);
                    audioSourceList.RemoveAt(i);
                    i--;

                }
            }
        }

        audioSource.Play();

        audioSourceList.Add(audioSource);
    }

    public void SetSFXVolume(float volume)
    {
        sfxMixer.audioMixer.SetFloat("SFXVolume", volume);
        SaveAudioOption();
    }

    public void SetMusicVolume(float volume)
    {
        musicMixer.audioMixer.SetFloat("MusicVolume", volume);
        SaveAudioOption();
    }

    public float GetSFXVolume()
    {
        float volume = 0.0f;
        sfxMixer.audioMixer.GetFloat("SFXVolume", out volume);
        return volume;
    }

    public float GetMusicVolume()
    {
        float volume = 0.0f;
        musicMixer.audioMixer.GetFloat("MusicVolume", out volume);
        return volume;
    }

    public void PauseSnapshot()
    {
        pauseSnapshot.TransitionTo(0.01f);
    }

    public void UnpauseSnapshot()
    {
        unpauseSnapshot.TransitionTo(0.1f);
    }

    public void SaveAudioOption()
    {
        string path = audioSavePath;
        StreamWriter streamWriter = new StreamWriter(path);
        if (streamWriter == null)
        {
            Debug.Log("Error : Can't write in " + path);
            return;
        }

        float sfxVolume = 0.0f;
        sfxMixer.audioMixer.GetFloat("SFXVolume", out sfxVolume);
        float musicVolume = 0.0f;
        musicMixer.audioMixer.GetFloat("MusicVolume", out musicVolume);

        streamWriter.WriteLine(sfxVolume);
        streamWriter.WriteLine(musicVolume);

        streamWriter.Close();
    }

    public void LoadAudioOption()
    {
        string path = audioSavePath;
        if (!File.Exists(path))
        {
            return;
        }
        StreamReader streamReader = new StreamReader(path);
        if (streamReader == null)
        {
            Debug.Log("Error : Can't read in " + path);
            return;
        }

        float sfxVolume = float.Parse(streamReader.ReadLine());
        float musicVolume = float.Parse(streamReader.ReadLine());

        sfxMixer.audioMixer.SetFloat("SFXVolume", sfxVolume);
        musicMixer.audioMixer.SetFloat("MusicVolume", musicVolume);

        streamReader.Close();
    }
}
