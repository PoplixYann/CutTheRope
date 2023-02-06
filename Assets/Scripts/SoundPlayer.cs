using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    [SerializeField] List<Sound> soundList = new List<Sound>();
    Dictionary<string, Sound> sounds = new Dictionary<string, Sound>();

    void Start()
    {
        for(int i = 0; i < soundList.Count; i++)
        {
            sounds.Add(soundList[i].name, soundList[i]);
        }
    }

    public void PlaySound(string name)
    {
        SoundManager.Instance.PlaySound(sounds[name]);
    }

    public void PlaySoundRandom(params string[] args)
    {
        int random = Random.Range(0, args.Length);
        PlaySound(args[random]);
    }
}
