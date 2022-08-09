using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomZRotation : MonoBehaviour
{
    public float min = -3;
    public float max = 3;

    void Start()
    {
        GetComponent<RectTransform>().localRotation = new Quaternion(0,0,Random.Range(min,max),90);   
    }
}
