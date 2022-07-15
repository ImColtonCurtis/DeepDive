using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectTracker : MonoBehaviour
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
        if (targetObj != null)
            myTransform.position = targetObj.position + offset;
        else
            Debug.Log("Error");
    }
}
