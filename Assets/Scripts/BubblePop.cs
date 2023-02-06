using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubblePop : MonoBehaviour
{
    public void DestroyBubble()
    {
        GetComponentInParent<Bubble>().DestroyBubbleGameObject();
    }
}
