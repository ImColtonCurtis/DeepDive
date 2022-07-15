using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLogic : MonoBehaviour
{
    int bounceCount, ricochetAmount;

    public GameObject sparkEffectObj, steamObj, tinyExplosionObj, dustExplosionObj;

    GameObject collidedObject, thisBulletTrail, tempObj;

    Transform effectsFolder, myTransform;

    Vector3 lastBouncePos = new Vector3(420, 420, 420);

    // Start is called before the first frame update
    void Awake()
    {
        myTransform = transform;

        bounceCount = 0;
        ricochetAmount = PlayerPrefs.GetInt("playerRicochetAmount", 1);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Wall")
        {
            if (Vector3.Distance(lastBouncePos, transform.position) > 0.5f)
            {

                if (bounceCount >= ricochetAmount)
                {
                    Destroy(gameObject);
                }
                else // ricochet
                {
                    tempObj = Instantiate(sparkEffectObj, myTransform.position, myTransform.rotation, effectsFolder);

                    Vector3 reflectDir = Vector3.Reflect(transform.forward, collision.GetContact(0).normal);
                    float rot = 90 - Mathf.Atan2(reflectDir.z, reflectDir.x) * Mathf.Rad2Deg;
                    myTransform.eulerAngles = new Vector3(0, rot, 0);

                    // velocity is 5 (maxspeed)
                    myTransform.GetComponent<Rigidbody>().velocity = 5 * myTransform.forward;
                    bounceCount++;
                }
            }
            lastBouncePos = transform.position;
        }
        else if (collision.transform.tag == "Bullet" || collision.transform.tag == "Tank")
        {
            Destroy(gameObject, Time.deltaTime);
        }
        collidedObject = collision.gameObject;
    }

    void LateUpdate()
    {
        if (thisBulletTrail != null)
            thisBulletTrail.transform.position = myTransform.position;
    }

    void OnEnable()
    {
        transform.GetComponent<SphereCollider>().enabled = false;
        StartCoroutine(EnableCollider());

        GameObject tempHolder;
        if (transform.parent.parent.gameObject != null)
            tempHolder = transform.parent.parent.gameObject;
        else
            tempHolder = null;

        if (tempHolder != null)
        {
            for (int i = 0; i < tempHolder.transform.childCount; i++)
            {
                if (transform.parent.parent.GetChild(i).tag == "EffectsFolder")
                    effectsFolder = transform.parent.parent.GetChild(i);
            }
        }
        thisBulletTrail = Instantiate(steamObj, transform.position, transform.rotation, effectsFolder);
    }

    void OnDisable()
    {
        if (collidedObject != null)
        {
            if (collidedObject.tag == "Bullet")
            {
                tempObj = Instantiate(tinyExplosionObj, transform.position, transform.rotation, effectsFolder);
            }
            else
            {
                tempObj = Instantiate(dustExplosionObj, transform.position, transform.rotation, effectsFolder);
            }
        }
        else
        {
            tempObj = Instantiate(dustExplosionObj, transform.position, transform.rotation, effectsFolder);
        }
    }

    IEnumerator EnableCollider()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        transform.GetComponent<SphereCollider>().enabled = true;
    }
}
