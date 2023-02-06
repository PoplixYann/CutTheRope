using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void GoToPlayMenu()
    {
        GameManager.Instance.GoToPlayMenu();
    }

    public void GoToOptionMenu()
    {
        GameManager.Instance.GoToOptionMenu();
    }

    public void Quit()
    {
        Application.Quit();
    }
}
