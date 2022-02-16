using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public Transform pivotPoint, topArrowPoint;
    public AnimationCurve slowMoCurve, ballSpeedCurve;
    public float timeToSlowMo, timeToReachMinSpeed;
    public float maxSpeed, forceRotation;

    private float timeToSlowMoCount, timeToReachMinSpeedCount;
    private bool clockWize = false;
    private Rigidbody rB;

    // Start is called before the first frame update
    void Start()
    {
        Screen.SetResolution(1080, 2400, true);
        rB = GetComponent<Rigidbody>();
    }

    // Update is called once per frame

    private void FixedUpdate()
    {
        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Stationary || Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                pivotPoint.Rotate(Vector3.up, forceRotation * Time.fixedDeltaTime * (1 / Time.timeScale) * (clockWize? 1 : -1));
            }
        }
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                clockWize = !clockWize;
                pivotPoint.gameObject.SetActive(true);
            }

            if (Input.GetTouch(0).phase == TouchPhase.Stationary || Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                pivotPoint.Rotate(Vector3.up, forceRotation * Time.deltaTime);
                DoSlowMo();
            }

            if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                pivotPoint.gameObject.SetActive(false);
                Vector3 direction = topArrowPoint.position - pivotPoint.position;
                direction = direction.normalized;
                direction.y = 0;
                StopSlowMo();

                GetComponent<Rigidbody>().velocity = direction * maxSpeed;
                timeToReachMinSpeedCount = 0;
            }
        }

        ConstantSpeed();
    }

    private void DoSlowMo()
        {
            float newTime = slowMoCurve.Evaluate(timeToSlowMoCount / timeToSlowMo);
            timeToSlowMoCount += Time.deltaTime * (1 / Time.timeScale);

            Time.timeScale = newTime;
            Time.fixedDeltaTime = newTime * 0.02f;
    }

    private void StopSlowMo()
    {
        timeToSlowMoCount = 0;
        Time.timeScale = 1;
        Time.fixedDeltaTime = 0.02f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        /*if (Time.time > timeCount)
        {
            Vector3 currentVelocity = rB.velocity.normalized;
            currentVelocity.y = 0;

            Vector3 normal = collision.contacts[0].normal;
            currentVelocity.y = 0;

            currentVelocity = Vector3.Reflect(currentVelocity, normal.normalized);

            rB.velocity = rB.velocity.magnitude * currentVelocity;

          timeCount = Time.time + 0.05f;
        */
    }

    private void ConstantSpeed()
    {
        timeToReachMinSpeedCount += Time.deltaTime;

        Vector3 direction = rB.velocity.normalized;

        rB.velocity = direction * maxSpeed * ballSpeedCurve.Evaluate(timeToReachMinSpeedCount/timeToReachMinSpeed);
    }
}
