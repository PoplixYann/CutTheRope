using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frog : MonoBehaviour
{
    Animator anim;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Ball")
        {
            Destroy(collision.gameObject);

            anim.SetBool("IsEat", true);

            Invoke("LevelIsWin", 2.0f);

            SoundPlayer sp = GetComponent<SoundPlayer>();
            sp.PlaySound("EatCandy");
        }
    }

    void LevelIsWin()
    {
        GameManager.Instance.UnlockNextLevel();
        GameManager.Instance.PlayNextLevel();
    }
}
