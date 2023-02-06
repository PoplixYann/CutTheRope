using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject optionPanel;
    [SerializeField] GameObject pausePanel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Continue()
    {
        GameManager.Instance.isPause = false;
        Time.timeScale = 1.0f;
        SoundManager.Instance.UnpauseSnapshot();

        gameObject.SetActive(false);
    }

    public void OpenLevelSelect()
    {
        GameManager.Instance.isPause = false;
        Time.timeScale = 1.0f;
        SoundManager.Instance.UnpauseSnapshot();

        GameManager.Instance.GoToPlayMenu();
    }

    public void OpenMainMenu()
    {
        GameManager.Instance.isPause = false;
        Time.timeScale = 1.0f;
        SoundManager.Instance.UnpauseSnapshot();

        GameManager.Instance.GoToMainMenu();
    }

    public void OpenOptionMenu()
    {
        pausePanel.SetActive(false);
        optionPanel.SetActive(true);
    }

    public void CloseOptionMenu()
    {
        pausePanel.SetActive(true);
        optionPanel.SetActive(false);
    }
}
