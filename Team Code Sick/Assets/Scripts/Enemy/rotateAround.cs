using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class rotateAround : MonoBehaviour
{

    public List<Transform> childObjects;
    public float rotationSpeed = 50f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < childObjects.Count; i++) 
       {
            childObjects[i].RotateAround(transform.position, transform.up, rotationSpeed * Time.deltaTime);
       }
    }
}

