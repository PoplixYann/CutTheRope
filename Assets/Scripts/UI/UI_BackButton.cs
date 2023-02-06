using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_BackButton : MonoBehaviour
{
    public void BackToMainMenu()
    {
        GameManager.Instance.GoToMainMenu();
    }
}
