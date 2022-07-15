using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlsLogic : MonoBehaviour
{
    bool leftTouchDown, rightTouchDown;

    [SerializeField] GameObject muzzleEffect;
    [SerializeField] Transform bulletsFolder, muzzleFlashFolder, effectsFolder;
    [SerializeField] Animator camAnim;

    // Base movement
    Vector3 anchorPosL, targetPosL, anchorPosR, targetPosR;
    Vector2 moveInputL;
    float targetAngleL, anchorRadiusL = 0.0192f/2f; // set anchor radius
    float moveSpeed;
    [SerializeField] PlayerController controller;

    // Bullet firing
    float targetAngleR, anchorRadiusR = 0.0192f/2f; // set anchor radius
    [SerializeField] Transform headTransform, bulletSpawnPos;
    bool recarchingBullet = false;
    float fireRate;
    GameObject tempBullet;
    [SerializeField] GameObject bulletObj;
    [SerializeField] Animator barrelAnim;

    void Awake()
    {
        leftTouchDown = false;
        rightTouchDown = false;

        moveSpeed = PlayerPrefs.GetFloat("PlayerMovementSpeed", 4.9f);
        fireRate = PlayerPrefs.GetFloat("PlayerFireRate", 0.5f);
    }

    void OnTouchDown(Vector3 point)
    {
        if (!leftTouchDown || !rightTouchDown)
        {            
            if (!GameManager.levelStarted)
                GameManager.levelStarted = true;
            if (!GameManager.levelFailed && !GameManager.levelPassed)
            {
                if (point.x <= 0 && !leftTouchDown)
                {
                    anchorPosL = new Vector3(point.x, point.y, anchorPosL.z);
                    targetPosL = new Vector3(point.x, point.y, targetPosL.z);
                    leftTouchDown = true;
                }
                else if (point.x > 0 && !rightTouchDown)
                {
                    anchorPosR = new Vector3(point.x, point.y, anchorPosR.z);
                    targetPosR = new Vector3(point.x, point.y, targetPosR.z);
                    rightTouchDown = true;
                }
            }
            
        }
    }

    void OnTouchStay(Vector3 point)
    {
        if (!GameManager.levelFailed)
        {
            // MOVEMENT
            if (leftTouchDown)
            {
                targetPosL = new Vector3(point.x, point.y, targetPosL.z);
                targetAngleL = Mathf.Rad2Deg * (Mathf.Atan2(targetPosL.y - anchorPosL.y, targetPosL.x - anchorPosL.x));
                if (targetAngleL < 0)
                    targetAngleL += 360;

                Vector3 moveDirection = new Vector3(Mathf.Cos(Mathf.Deg2Rad * targetAngleL), Mathf.Sin(Mathf.Deg2Rad * targetAngleL), targetPosL.z);
                moveInputL = new Vector2(Mathf.Clamp(moveDirection.x, -1, 1) * Mathf.Clamp(Mathf.Abs(targetPosL.x - anchorPosL.x) / anchorRadiusL, 0, 1),
                    Mathf.Clamp(moveDirection.y, -1, 1) * Mathf.Clamp(Mathf.Abs(targetPosL.y - anchorPosL.y) / anchorRadiusL, 0, 1));

                Vector3 moveVelocity = moveInputL * moveSpeed;
                controller.Move(moveVelocity);
                controller.LookAt(targetAngleL);

                // if finger position is greater than (anchorRadius * 2), then reset anchor to be (anchorRadius)
                if (Vector3.Distance(anchorPosL, targetPosL) >= (anchorRadiusL * 2))
                    anchorPosL = Vector3.Lerp(anchorPosL, targetPosL, 0.5f);
            }
            // AIM
            if (rightTouchDown)
            {
                targetPosR = new Vector3(point.x, point.y, targetPosR.z);
                targetAngleR = Mathf.Rad2Deg * (Mathf.Atan2(targetPosR.y - anchorPosR.y, targetPosR.x - anchorPosR.x));
                if (targetAngleR < 0)
                    targetAngleR += 360;
               
                headTransform.eulerAngles = new Vector3(-90, -targetAngleR + 90, 0);

                // fire bullets
                if (!recarchingBullet)
                    StartCoroutine(FireBullet());             

                // if finger position is greater than (anchorRadius * 2), then reset anchor to be (anchorRadius)
                if (Vector3.Distance(anchorPosR, targetPosR) >= (anchorRadiusR * 2))
                    anchorPosR = Vector3.Lerp(anchorPosR, targetPosR, 0.5f);
            }
        }
    }

    IEnumerator FireBullet()
    {
        recarchingBullet = true;

        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();

        Instantiate(muzzleEffect, muzzleFlashFolder.transform.position, muzzleFlashFolder.transform.rotation, muzzleFlashFolder);

        barrelAnim.SetTrigger("Fire");

        tempBullet = Instantiate(bulletObj, bulletSpawnPos.transform.position, bulletSpawnPos.transform.rotation, bulletsFolder);
        tempBullet.transform.localEulerAngles = new Vector3(0, tempBullet.transform.localEulerAngles.y, 0);
        tempBullet.GetComponent<Rigidbody>().velocity = Vector3.zero;
        tempBullet.GetComponent<Rigidbody>().velocity = 5 * bulletSpawnPos.transform.forward;

        yield return new WaitForSeconds(fireRate);

        recarchingBullet = false;
    }

    void OnTouchUp()
    {
        if (GameManager.levelStarted && !GameManager.levelFailed && !GameManager.levelPassed)
        {
            if (leftTouchDown)
            {
                moveInputL = Vector2.zero;
                Vector3 moveVelocity = moveInputL.normalized * moveSpeed;
                controller.Move(moveVelocity);

                leftTouchDown = false;
            }
            if (rightTouchDown)
            {
                rightTouchDown = false;
            }
        }
    }

    void OnTouchExit()
    {
        if (GameManager.levelStarted && !GameManager.levelFailed && !GameManager.levelPassed)
        {
            if (leftTouchDown)
            {
                moveInputL = Vector2.zero;
                Vector3 moveVelocity = moveInputL.normalized * moveSpeed;
                controller.Move(moveVelocity);

                leftTouchDown = false;
            }
            if (rightTouchDown)
            {
                rightTouchDown = false;
            }
        }
    }
}
