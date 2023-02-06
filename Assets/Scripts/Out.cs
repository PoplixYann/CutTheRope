using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Out : MonoBehaviour
{
    bool isCol = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isCol && collision.tag == "Ball")
        {
            isCol = true;
            Invoke("ReplayLevel", 0.5f);
        }
    }

    void ReplayLevel()
    {
        GameManager.Instance.ReplayLevel();
    }
}
