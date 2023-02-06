using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerRope : MonoBehaviour
{
    [SerializeField] GameObject triggerVisual;
    public bool isTrigger = false;

    void Start()
    {
        GetComponent<RopeGenerator>().visual.SetActive(false);
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Ball" && !isTrigger)
        {
            Ball ball = collision.GetComponent<Ball>();
            ball.JointToRope(gameObject);

            GetComponent<CircleCollider2D>().enabled = false;

            GetComponent<RopeGenerator>().visual.SetActive(true);
            triggerVisual.SetActive(false);

            isTrigger = true;

            SoundPlayer sp = GetComponent<SoundPlayer>();
            sp.PlaySound("RopeGet");
        }
    }

    /*Call in editor when rope param are modified*/
    public void UpdateTriggerZoneSize()
    {
        RopeGenerator ropeGenerator = GetComponent<RopeGenerator>();
        CircleCollider2D col = GetComponent<CircleCollider2D>();
        col.radius = ropeGenerator.size - 0.2f;

        GameObject triggerZone = transform.Find("TriggerZone").gameObject;
        GameObject triggerZoneVisual = triggerZone.transform.Find("Visual").gameObject;
        triggerZoneVisual.transform.localScale = new Vector3((ropeGenerator.size-0.2f)*2.0f, (ropeGenerator.size-0.2f)*2.0f, 1);
    }
}
