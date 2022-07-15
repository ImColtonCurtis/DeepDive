using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAnimation : MonoBehaviour
{
    [SerializeField] Animator anim;
    // Start is called before the first frame update
    void Awake()
    {
        anim.Play("Windmill_Anim", 0, Random.Range(0f, 1f));
    }
}
