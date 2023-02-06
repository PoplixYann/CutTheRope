using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowBall : MonoBehaviour
{
    [SerializeField] GameObject ball;
    [SerializeField] GameObject minY;
    [SerializeField] GameObject maxY;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (ball == null)
            return;

        Vector3 newPos = new Vector3(transform.position.x, ball.transform.position.y, transform.position.z);

        if (newPos.y + Camera.main.orthographicSize > maxY.transform.position.y)
        {
            newPos.y = maxY.transform.position.y - Camera.main.orthographicSize;
        }
        else if (newPos.y - Camera.main.orthographicSize < minY.transform.position.y)
        {
            newPos.y = minY.transform.position.y + Camera.main.orthographicSize;
        }

        transform.position = Vector3.Lerp(transform.position, newPos, Time.deltaTime * 5.0f);
    }
}
