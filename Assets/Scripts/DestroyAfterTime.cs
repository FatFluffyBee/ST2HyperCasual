using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    public float min = 2, max = 3;

    void Start()
    {
        Destroy(gameObject, Random.Range(min, max));
    }
}
