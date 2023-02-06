using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinePoint
{
    public Vector3 pos;
    public float time;
    public int id;
}

public class Controller : MonoBehaviour
{

    [SerializeField] LineRenderer line;
    List<LinePoint> linePoints = new List<LinePoint>();


    Vector2 curMousePos = Vector2.zero;
    Vector2 prevMousePos = Vector2.zero;
    Ball ball = null;

    bool mouseIsPressed = false;

    float timer = 0f;
    float timerMax = 0.1f;
    bool isBubbleClick = false;

    void Start()
    {
        ball = GameObject.FindGameObjectWithTag("Ball").GetComponent<Ball>();
    }

    void Update()
    {
        if (GameManager.Instance.isPause)
        {
            return;
        }

        if (isBubbleClick)
        {
            timer -= Time.deltaTime;
            if (timer <= 0.0f)
            {
                isBubbleClick = false;
            }
        }

        if (mouseIsPressed)
        {
            LinePoint lineP = new LinePoint();
            lineP.pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            lineP.pos = new Vector3(lineP.pos.x, lineP.pos.y, 0.0f);
            lineP.time = 0.1f;
            linePoints.Add(lineP);
            line.positionCount++;
            line.SetPosition(line.positionCount - 1, lineP.pos);

            for (int i = 0; i < linePoints.Count; i++)
            {
                linePoints[i].time -= Time.deltaTime;
                if (linePoints[i].time <= 0.0f)
                {
                    linePoints.Remove(linePoints[i]);
                    line.positionCount--;
                    Vector3[] lpointsPos = new Vector3[linePoints.Count];
                    for (int j = 0; j < linePoints.Count; j++)
                    {
                        lpointsPos[j] = linePoints[j].pos;
                    }
                    line.SetPositions(lpointsPos);
                    i--;
                }
            }
        }

        if (Input.GetMouseButton(0) && !isBubbleClick)
        {
            MousePressed();
        }
        if (Input.GetMouseButtonUp(0))
        {
            MouseReleased();
        }
    }

    //Mouse left click is pressed
    void MousePressed()
    {
        curMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        RaycastHit2D[] hits;
        if (mouseIsPressed) //If isn't first frame of pressed
        {
            hits = Physics2D.RaycastAll(curMousePos, prevMousePos - curMousePos, (prevMousePos - curMousePos).magnitude);
        }
        else //If is first frame of pressed
        {
            hits = Physics2D.RaycastAll(curMousePos, Vector2.zero);
        }

        if (hits.Length > 0)
        {
            RaycastHit2D wantHit = hits[0];
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider != null)
                {
                    if (!mouseIsPressed && hit.collider.tag == "Bubble")
                    {
                        wantHit = hit;
                    }
                    else if (hit.collider.tag == "RopePart" && wantHit.collider?.tag != "Bubble")
                    {
                        wantHit = hit;
                    }
                }
            }

            if (!mouseIsPressed && wantHit.collider.tag == "Bubble")
            {
                isBubbleClick = true;
                timer = timerMax;
                wantHit.collider.GetComponent<Bubble>().DestroyBubble();
            }
            else if (wantHit.collider.tag == "RopePart")
            {
                bool canCut = true;
                TriggerRope triggerRope = null;
                wantHit.collider.transform.parent.parent.parent.TryGetComponent<TriggerRope>(out triggerRope);
                if (triggerRope != null)
                {
                    canCut = triggerRope.isTrigger;
                }

                if (!wantHit.collider.gameObject.GetComponentInParent<RopeGenerator>().isCut && canCut)
                {
                    RopeGenerator rope = wantHit.collider.gameObject.GetComponentInParent<RopeGenerator>();
                    GameObject ropeGO = rope.gameObject;

                    rope.CutRope(wantHit.collider.gameObject);

                    ball.UnjointToRope(ropeGO);
                }
            }
        }


        prevMousePos = curMousePos;
        mouseIsPressed = true;
    }

    //Mouse left click is released
    void MouseReleased()
    {
        mouseIsPressed = false;
        linePoints.Clear();
        line.positionCount = 0;
    }
}
