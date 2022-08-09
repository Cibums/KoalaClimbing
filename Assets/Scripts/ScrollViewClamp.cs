using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollViewClamp : MonoBehaviour
{
    public RectTransform content;

    public float minY;
    public float maxY;

    void Update()
    {
        if (content.anchoredPosition.y < minY)
        {
            content.anchoredPosition = new Vector2(0,minY);
        }

        if (content.anchoredPosition.y > maxY)
        {
            content.anchoredPosition = new Vector2(0, maxY);
        }

    }
}
