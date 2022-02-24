using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    public float timeBeforeDestruction;

    void Start()
    {
        Destroy(gameObject, timeBeforeDestruction);
    }
}
