using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    Animator anim;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        anim.speed = Random.Range(2.0f, 2.5f);
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Ball")
        {
            GameManager.Instance.CurLevelManager.StarsCount++;
            anim.SetBool("IsTake", true);

            SoundPlayer sp = GetComponent<SoundPlayer>();
            string soundToPlay = "Star" + GameManager.Instance.CurLevelManager.StarsCount;
            sp.PlaySound(soundToPlay);
        }
    }

    public void DestroyStarGameObject()
    {
        Destroy(gameObject);
    }
}
