using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Ball")
        {
            Ball ball = collision.GetComponent<Ball>();
            ball.DestroyBall();

            Invoke("ReplayLevel", 0.5f);
        }
    }

    void ReplayLevel()
    {
        GameManager.Instance.ReplayLevel();
    }
}
