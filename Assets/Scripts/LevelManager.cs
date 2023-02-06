using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public event System.Action starsCountEvent;
    int starsCount = 0;
    public int StarsCount
    {
        get => starsCount;
        set
        {
            starsCount = value;
            starsCountEvent?.Invoke();
        }
    }

    void Start()
    {

    }

    void Update()
    {

    }
}
