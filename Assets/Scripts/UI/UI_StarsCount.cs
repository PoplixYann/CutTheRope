using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_StarsCount : MonoBehaviour
{
    [SerializeField] Sprite[] star = new Sprite[4];

    [SerializeField] Image starsCountImg;

    void Start()
    {
        GameManager.Instance.CurLevelManager.starsCountEvent += UpdateStarsCount;
    }

    void Update()
    {
        
    }

    void UpdateStarsCount()
    {
        starsCountImg.sprite = star[GameManager.Instance.CurLevelManager.StarsCount];
    }
}
