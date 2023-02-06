using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_PauseButton : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void OpenPauseMenu()
    {
        pauseMenu.SetActive(true);
        GameManager.Instance.isPause = true;
        Time.timeScale = 0.0f;
        SoundManager.Instance.PauseSnapshot();
    }
}
