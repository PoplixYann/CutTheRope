using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    Animator anim = null;

    [SerializeField] float gravityScale = 0.5f;
    [SerializeField] float speed = 1.0f;
    [SerializeField] float velocityYLimit = 1.0f;
    [SerializeField] float drag = 5.0f;

    bool isPop = false;
    bool isTrigger = false;
    bool isDestroy = false;
    Ball ball = null;
    Rigidbody2D ballRB = null;
    float prevBallDrag = 0.05f;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (isTrigger && !isPop)
        {
            if (ball == null)
            {
                anim.SetBool("IsPop", true);
                isPop = true;
                isDestroy = true;
                return;
            }

            transform.position = ball.transform.position;
            ballRB.gravityScale = Mathf.Lerp(ballRB.gravityScale, -gravityScale, speed * Time.deltaTime);
            if (ballRB.velocity.y > velocityYLimit)
            {
                ballRB.velocity = new Vector2(ballRB.velocity.x, velocityYLimit);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Ball" && !isTrigger)
        {
            ball = collision.GetComponent<Ball>();
            ballRB = collision.GetComponent<Rigidbody2D>();
            //ballRB.velocity = new Vector2(0.0f, ballRB.velocity.y);
            prevBallDrag = ballRB.drag;
            ballRB.drag = drag;
            ball.transform.position = new Vector2(transform.position.x, ball.transform.position.y);
            isTrigger = true;
            anim.SetBool("IsFlying", true);

            SoundPlayer sp = GetComponent<SoundPlayer>();
            sp.PlaySound("BubbleCatch");
        }
    }

    public void DestroyBubble()
    {
        if (isTrigger && !isDestroy)
        {
            Rigidbody2D ballRB = ball.GetComponent<Rigidbody2D>();
            ballRB.velocity = Vector2.zero;
            ballRB.gravityScale = 1;
            ballRB.drag = prevBallDrag;
            isPop = true;
            anim.SetBool("IsPop", true);
            isDestroy = true;

            SoundPlayer sp = GetComponent<SoundPlayer>();
            sp.PlaySound("BubbleBreak");
        }
    }

    public void DestroyBubbleGameObject()
    {
        Destroy(gameObject);
    }
}
