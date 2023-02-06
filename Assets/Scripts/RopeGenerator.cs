using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class RopeGenerator : MonoBehaviour
{
    [Header("Part Prefab")]
    [SerializeField] GameObject partParent;
    [SerializeField] GameObject part;

    [Header("Part Settings")]
    [Min(0.01f)]
    [SerializeField] float partSize = 0.5f;

    [Min(0.0f)]
    [SerializeField] float partDistance = 0.0f;

    [Header("Rope Settings")]
    [Range(0.0f, 10.0f)]
    [SerializeField] public float size = 2.0f;

    [Header("Rope Reference")]
    public GameObject ropeRef;

    [Header("Part Reference")]
    public List<GameObject> partsList = new List<GameObject>();
    public GameObject lastPart = null;
    public int cutPartId = 0;

    [Header("Cut Settings")]
    public bool isCut = false;
    [SerializeField] float timerToDestroy = 1.0f;
    [SerializeField] float timerToDestroyMax;
    bool isDestroy = false;

    [Header("Ball Reference")]
    public Ball ball = null;

    [Header("Line Renderer")]
    public GameObject visual = null;
    [SerializeField] GameObject visual2 = null;

    void Start()
    {
        timerToDestroyMax = timerToDestroy;
    }

    void Update()
    {
        if (!isDestroy)
        {
            CutUpdate();
            UpdateLineRenderer();
        }
    }

    void CutUpdate()
    {
        /*Rope is cut*/
        if (isCut)
        {
            /*Destroy all parts and visual after timer*/
            timerToDestroy -= Time.deltaTime;
            if (timerToDestroy <= 0.0f)
            {
                foreach (var part in partsList)
                {
                    Destroy(part.gameObject);
                }
                partsList.Clear();
                lastPart = null;

                Destroy(visual);
                visual = null;
                Destroy(visual2);
                visual2 = null;

                isDestroy = true;
            }

            if (!isDestroy)
            {
                /*Change alpha of visual line renderer*/
                LineRenderer lr1 = visual.GetComponent<LineRenderer>();
                LineRenderer lr2 = visual2.GetComponent<LineRenderer>();

                Color color = lr1.startColor;

                color.a = timerToDestroy / timerToDestroyMax;
                lr1.startColor = color;
                lr1.endColor = color;

                lr2.startColor = color;
                lr2.endColor = color;

                /*Make last part follow ball (useful to have visual and physics of rope following ball after cut without joint with ball*/
                if (lastPart != null && ball != null)
                {
                    lastPart.GetComponent<Rigidbody2D>().MovePosition(ball.GetComponent<Rigidbody2D>().position);
                    lastPart.GetComponent<Rigidbody2D>().MoveRotation(ball.GetComponent<Rigidbody2D>().rotation);
                }
            }
        }
    }

    public void CutRope(GameObject cutedPart)
    {
        LineRenderer lr1 = visual.GetComponent<LineRenderer>();
        LineRenderer lr2 = visual2.GetComponent<LineRenderer>();

        /*Find id of the cuted part to separe both line renderer*/
        cutPartId = partsList.FindIndex(x => x.Equals(cutedPart));

        lr1.positionCount = cutPartId;
        lr2.positionCount = partsList.Count - cutPartId;

        visual2.SetActive(true);

        Destroy(cutedPart.GetComponent<HingeJoint2D>());
        isCut = true;

        SoundPlayer sp = GetComponent<SoundPlayer>();
        sp.PlaySoundRandom("RopeCut1", "RopeCut2");
    }

    void UpdateLineRenderer()
    {
        if (isDestroy)
            return;

        if (isCut) //Update position of both line renderer
        {
            LineRenderer lr1 = visual.GetComponent<LineRenderer>();
            LineRenderer lr2 = visual2.GetComponent<LineRenderer>();

            for (int i = 0; i < lr1.positionCount; i++)
            {
                lr1.SetPosition(i, partsList[i].transform.localPosition);
            }
            for (int i = 0; i < lr2.positionCount; i++)
            {
                lr2.SetPosition(i, partsList[i + cutPartId].transform.localPosition);
            }
        }
        else //Update position of first line renderer
        {
            LineRenderer lr = visual.GetComponent<LineRenderer>();

            for (int i = 0; i < lr.positionCount; i++)
            {
                lr.SetPosition(i, partsList[i].transform.localPosition);
            }
        }
    }

    //Function call when rope or part settings is modified
    //This function delete previous parts and create new parts with new settings
    public void GenerateRope()
    {
        /*Destroy previous parts*/
        for (int i = 0; i < partParent.transform.childCount; i++)
        {
            if (Application.isPlaying)
            {
                Destroy(partParent.transform.GetChild(i).gameObject);
            }
            else if (Application.isEditor)
            {
                DestroyImmediate(partParent.transform.GetChild(i).gameObject);
                i--;
            }
        }

        partsList.Clear();

        float curSize = 0.0f;

        GameObject curPart = null;
        Rigidbody2D prevPartRB = null;

        if (partDistance < partSize)
        {
            partDistance = partSize;
        }

        /*Create new parts*/
        while (curSize <= size)
        {
            /*Create one part*/
            Vector3 partPos = new Vector3(partParent.transform.position.x, partParent.transform.position.y - curSize, 0.0f);
            curPart = Instantiate(part, partPos, Quaternion.identity);
            curPart.transform.parent = partParent.transform;

            GameObject curPartVisual = curPart.transform.Find("Visual").gameObject;
            CircleCollider2D curPartCol = curPart.GetComponent<CircleCollider2D>();
            Rigidbody2D curPartRB = curPart.GetComponent<Rigidbody2D>();
            HingeJoint2D curPartJoint = curPart.GetComponent<HingeJoint2D>();

            /*Scale visual to partSize*/
            Vector3 newScale = curPartVisual.transform.localScale;
            newScale.y = partSize;
            curPartVisual.transform.localScale = newScale;

            /*Change radius of circle collider*/
            float newRadius = curPartCol.radius;
            newRadius = partDistance / 2.0f;
            curPartCol.radius = newRadius;

            /*If not first part then setup joint*/
            if (prevPartRB != null)
            {
                curPartJoint.connectedBody = prevPartRB;
                curPartJoint.autoConfigureConnectedAnchor = false;
                Vector2 newConnectedAnchor = curPartJoint.connectedAnchor;
                newConnectedAnchor.y = -partDistance;
                curPartJoint.connectedAnchor = newConnectedAnchor;
            }

            prevPartRB = curPartRB;

            curSize += partDistance;

            partsList.Add(curPart);
        }

        /*Generate first line renderer*/
        LineRenderer curLr = visual.GetComponent<LineRenderer>();
        curLr.positionCount = partsList.Count;
        UpdateLineRenderer();

        lastPart = curPart;

    }
}
