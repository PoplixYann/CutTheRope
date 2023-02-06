using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarTake : MonoBehaviour
{
    public void DestroyStar()
    {
        GetComponentInParent<Star>().DestroyStarGameObject();
    }
}
