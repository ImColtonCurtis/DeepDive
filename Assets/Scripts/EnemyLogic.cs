using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLogic : MonoBehaviour
{
    [SerializeField] GameObject tankObj, tankExplosion;
    [SerializeField] Transform effectsFolder;

    Vector3 velocity;
    [SerializeField] Rigidbody myRigidBody;
    Transform myTransform;
    
    // Enemy Stats
    [SerializeField] float moveSpeed;
    [SerializeField] float fireRate;
    [SerializeField] float ricochetAmount;
    [SerializeField] float enemyPrecision;
    [SerializeField] float dodgeAbility;
    [SerializeField] float offensiveDefensiveAmount;

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
        if (collision.gameObject.tag == "Bullet" && !GameManager.levelFailed && !GameManager.levelPassed)
        {
            // Lost
            tankObj.SetActive(false);
        }
    }

    void OnDisable()
    {
        Instantiate(tankExplosion, transform.position, transform.rotation, effectsFolder);
    }
}
