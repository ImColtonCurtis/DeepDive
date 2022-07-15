using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Tracker : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Vector3 offset;

    float smoothTime = 0.05f;
    Vector3 velocity = Vector3.zero;

    Transform myTransform;

    public static bool stopCameraTracking;
    bool cameraStopped;

    private void Awake()
    {
        myTransform = transform;
        stopCameraTracking = false;
        cameraStopped = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Define a target position above and behind the target transform
        Vector3 targetPosition = Vector3.zero;
        if (target != null)
            targetPosition = target.position + offset;
        targetPosition = new Vector3(targetPosition.x, 0, targetPosition.z);

        // Smoothly move the camera towards that target position
        if (!GameManager.levelPassed &&  !cameraStopped)
            myTransform.localPosition = Vector3.SmoothDamp(myTransform.localPosition, targetPosition, ref velocity, smoothTime);        
    }

    private void Update()
    {
        if (stopCameraTracking)
        {
            StartCoroutine(WaitForTime());
            stopCameraTracking = false;
        }
    }

    IEnumerator WaitForTime()
    {
        yield return new WaitForSeconds(0.8f);
        cameraStopped = true;
    }
}
