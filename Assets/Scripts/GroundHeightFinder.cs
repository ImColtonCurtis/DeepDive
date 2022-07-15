using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundHeightFinder : MonoBehaviour
{
    public LayerMask mask;

    Transform myTransform;

    public Vector3 direction;

    public void Start()
    {
        myTransform = transform;
    }

    public Vector3 GetGroundHeight()
    {
        Ray ray = new Ray(new Vector3(myTransform.localPosition.x, myTransform.localPosition.y + 0.5f, myTransform.localPosition.z), -myTransform.up);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, 100, mask, QueryTriggerInteraction.Ignore))
        {
            return hitInfo.point;
        }
        return Vector3.zero;
    }
}
