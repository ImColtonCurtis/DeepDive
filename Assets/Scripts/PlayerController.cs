using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameObject tankObj, tankExplosion;
    [SerializeField] Transform effectsFolder;

    Vector3 velocity;
    [SerializeField] Rigidbody myRigidBody;
    Transform myTransform;

    private void Start()
    {
        myTransform = transform;
    }

    public void Move(Vector3 _velocity)
    {
        velocity = new Vector3(_velocity.x, velocity.y, _velocity.y);
    }

    public void LookAt(float _rotation)
    {
        myTransform.eulerAngles = new Vector3(0, -_rotation + 90, 0);
    }

    public void FixedUpdate()
    {
        var pos = myRigidBody.position + velocity * Time.fixedDeltaTime;

        if (!GameManager.levelFailed)
        {
            myRigidBody.MovePosition(pos);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Win" && !GameManager.levelFailed && !GameManager.levelPassed)
        {
            GameManager.levelPassed = true;
            ShowConfetti.confObj.SetActive(true);
        }

        if (collision.gameObject.tag == "Bullet" && !GameManager.levelFailed && !GameManager.levelPassed)
        {
            // Lost
            tankObj.SetActive(false);
            //GameManager.levelFailed = true;
        }
    }

    void OnDisable()
    {
        Instantiate(tankExplosion, transform.position, transform.rotation, effectsFolder);
    }
}
