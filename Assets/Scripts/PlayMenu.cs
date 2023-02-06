using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayMenu : MonoBehaviour
{
    [SerializeField] Sprite lockSpr;
    [SerializeField] Sprite unlockSpr;

    [SerializeField] Sprite[] star = new Sprite[4];

    public List<GameObject> levelButList = new List<GameObject>();

    void Start()
    {
        for (int i = 0; i < levelButList.Count; i++)
        {
            Button but = levelButList[i].GetComponent<Button>();
            Image starImg = but.transform.Find("Star").GetComponent<Image>();

            if (!GameManager.Instance.levelList[i].isUnlock)
            {
                but.interactable = false;
                but.image.sprite = lockSpr;
                but.GetComponentInChildren<Text>().enabled = false;
                starImg.enabled = false;
            }
            else
            {
                but.interactable = true;
                but.image.sprite = unlockSpr;
                but.GetComponentInChildren<Text>().enabled = true;
                starImg.enabled = true;
                starImg.sprite = star[GameManager.Instance.levelList[i].nbStars];

            }
        }
    }

    public void PlayLevel(int id)
    {
        GameManager.Instance.PlayLevel(id);
    }

}
