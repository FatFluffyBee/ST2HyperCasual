using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BumperBehavior : MonoBehaviour
{
    public Material matOn, matOff;
    public AnimationCurve curve;
    public float durationFeedback, numberBleep;
    public float modifVelocity = 1.5f;
    public int bumperCombo = 0;

    private float durationFeedbackCount;
    private MeshRenderer mR;

    FMOD.Studio.EventInstance sound;

    private void Start()
    {
        mR = transform.GetChild(0).GetComponent<MeshRenderer>();

        durationFeedbackCount = durationFeedback;

        sound = FMODUnity.RuntimeManager.CreateInstance("event:/Ball/Bl_BulletTime/Bl_BulletTime");
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
            bumperCombo++;
            FMODUnity.RuntimeManager.PlayOneShot("event:/Bumper/Bump_Combo/Bump_Combo");
        }
    }


}
