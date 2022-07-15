using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailTracker : MonoBehaviour
{
    [SerializeField] Transform targetObj;
    [SerializeField] Vector3 offset;
    Transform myTransform;

    // Start is called before the first frame update
    void Start()
    {
        myTransform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        myTransform.position = targetObj.position + offset; //3.5171,  2.0977
        myTransform.localPosition = new Vector3(myTransform.localPosition.x, -6.4f, myTransform.localPosition.z);
    }
}
