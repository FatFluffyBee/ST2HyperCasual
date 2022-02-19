using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BumperBehavior : MonoBehaviour
{
    public Material matOn, matOff;
    public AnimationCurve curve;
    public float durationFeedback, numberBleep;
    public float modifVelocity = 1.5f;

    private float durationFeedbackCount;
    private MeshRenderer mR;

    private void Start()
    {
        mR = transform.GetChild(0).GetComponent<MeshRenderer>();

        durationFeedbackCount = durationFeedback;
    }

    void Update()
    {
        if (durationFeedbackCount < durationFeedback)
        {
            float tmp = (durationFeedbackCount / (durationFeedback / numberBleep))%1;

            mR.materials[1].Lerp(matOn, matOff, curve.Evaluate(tmp));
            durationFeedbackCount += Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player")
        {
            collision.collider.GetComponent<Rigidbody>().velocity *= modifVelocity;
            durationFeedbackCount = 0;
        }
    }
}
