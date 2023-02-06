using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Ball : MonoBehaviour
{
    Rigidbody2D rb = null;
    [SerializeField] GameObject[] ballPart = new GameObject[5];
    [SerializeField] GameObject[] startRopes = new GameObject[0];
    Dictionary<GameObject, HingeJoint2D> ropesJointDictionary = new Dictionary<GameObject, HingeJoint2D>();

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        for(int i = 0; i < startRopes.Length; i++)
        {
            JointToRope(startRopes[i]);
        }
    }

    void Update()
    {
        
    }

    public void DestroyBall()
    {
        for(int i = 0; i < ballPart.Length; i++)
        {
            GameObject part = Instantiate(ballPart[i], transform.position, Quaternion.identity);
            Rigidbody2D partRB = part.GetComponent<Rigidbody2D>();
            part.transform.position += new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(0.0f, 0.1f), 0.0f);
            partRB.AddForce(new Vector2(Random.Range(-100.0f, 100.0f), Random.Range(50.0f, 150.0f)));
        }

        SoundPlayer sp = GetComponent<SoundPlayer>();
        sp.PlaySound("CandyBreak");

        Destroy(gameObject);
    }

    //Joint ball to a rope
    public void JointToRope(GameObject ropeGO)
    {
        HingeJoint2D curJoint = gameObject.AddComponent<HingeJoint2D>();
        RopeGenerator rope = ropeGO.GetComponent<RopeGenerator>();
        GameObject lastRopePart = rope.lastPart;
        GameObject ropeRef = rope.ropeRef;

        /*Rotate rope to look at ball*/
        float angle = Mathf.Atan2(transform.position.y - ropeRef.transform.position.y, transform.position.x - ropeRef.transform.position.x) * Mathf.Rad2Deg;
        ropeRef.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 90.0f));

        lastRopePart.GetComponent<Rigidbody2D>().position = rb.position;

        /*Setup joint*/
        curJoint.connectedBody = lastRopePart.GetComponent<Rigidbody2D>();
        curJoint.autoConfigureConnectedAnchor = false;
        curJoint.connectedAnchor = new Vector2(0.0f, 0.0f);

        /*Ball reference in rope script*/
        rope.ball = this;

        ropesJointDictionary.Add(ropeGO, curJoint);
    }

    //Unjoint ball to a rope, which is already joint
    public void UnjointToRope(GameObject rope)
    {
        if (ropesJointDictionary.ContainsKey(rope))
        {
            HingeJoint2D curJoint = ropesJointDictionary[rope];
            Destroy(curJoint);
            ropesJointDictionary.Remove(rope);
        }
    }
}
