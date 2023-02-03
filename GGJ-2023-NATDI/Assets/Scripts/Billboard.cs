using System;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    void Update()
    {
        transform.rotation = Camera.main.transform.rotation;
        //transform.Rotate(Vector3.up * 180f);
    }
}
