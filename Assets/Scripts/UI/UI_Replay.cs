using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Replay : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void ReplayLevel()
    {
        GameManager.Instance.PlayLevel(GameManager.Instance.curLevel);
    }
}
