using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleCube : MonoBehaviour
{
    public enum statesCollectibleCube {Active, Deactive, MiddlePoint}
    public statesCollectibleCube state = statesCollectibleCube.Deactive;

    public Material colorActive, colorDeactivate;
    public AnimationCurve curveActive, curveDeactivate, curveMiddlePoint;
    public float timer = 2f;

    private MeshRenderer mR;
    public  float timerCount;

    // Start is called before the first frame update
    void Start()
    {
        mR = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == statesCollectibleCube.Deactive)
        {
            if (timerCount > timer) { timerCount = 0; }

            mR.material.Lerp(colorDeactivate, colorActive, curveDeactivate.Evaluate(timerCount/timer));
            timerCount += Time.deltaTime;
        }

        if (state == statesCollectibleCube.Active)
        {
            if (timerCount > timer) { timerCount = 0; }

            mR.material.Lerp(colorDeactivate, colorActive, curveActive.Evaluate(timerCount / timer));
            timerCount += Time.deltaTime;
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player")
        {
            state = statesCollectibleCube.Active;
            timerCount = 0;
        }
    }

    public void Reset()
    {
        state = statesCollectibleCube.Deactive;
        timerCount = 0;
    }
}
