using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum SoundType
{
    SFX,
    MUSIC
}

[Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    public SoundType type;
}
