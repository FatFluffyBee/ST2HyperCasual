using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public GameObject arrow, pivotArrow;
    public AnimationCurve slowMoCurve, ballSpeedCurve;
    public float timeToSlowMo, timeToReachMinSpeed;
    public float maxSpeed, forceRotation;
    public float maxDistanceForDrag, maxArrowScale;

    private float timeToSlowMoCount, timeToReachMinSpeedCount;
    private Rigidbody rB;
    private GameObject startPoint, endPoint;
    private float releaseSpeed;

    // Start is called before the first frame update
    void Start()
    {
        Screen.SetResolution(1080, 2400, true);
        rB = GetComponent<Rigidbody>();

        startPoint = GameObject.Find("StartPoint");
        endPoint = GameObject.Find("EndPoint");
    }

    // Update is called once per frame

    void Update()
    {
        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                startPoint.SetActive(true);
                endPoint.SetActive(true);
                arrow.SetActive(true);
                Vector3 positionStartPoint = Input.GetTouch(0).position;
                startPoint.transform.position = positionStartPoint;
            }

            if (Input.GetTouch(0).phase == TouchPhase.Stationary || Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                Vector3 tmp = Input.GetTouch(0).position;

                if (Vector3.Distance(tmp, startPoint.transform.position) > maxDistanceForDrag) {
                    endPoint.transform.position = startPoint.transform.position + (tmp - startPoint.transform.position).normalized * maxDistanceForDrag;}
                else { endPoint.transform.position = tmp;}

                Vector3 arrowScale = arrow.transform.localScale;
                arrowScale.y = Vector3.Distance(endPoint.transform.position, startPoint.transform.position) / maxDistanceForDrag * maxArrowScale;
                arrow.transform.localScale = arrowScale;

                Vector3 direction = endPoint.transform.position - startPoint.transform.position;
                direction.z = direction.y;
                direction.y = 0;

                Vector3 arrowPosition = Vector3.zero;
                arrowPosition.z = arrow.transform.localScale.y;
                arrow.transform.position = transform.position - direction.normalized * arrow.transform.localScale.y;

                
                pivotArrow.transform.LookAt(transform.position - direction.normalized, Vector3.up);

                DoSlowMo();
            }

            if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                startPoint.SetActive(false);
                endPoint.SetActive(false);
                arrow.SetActive(false);

                Vector3 direction = startPoint.transform.position - endPoint.transform.position;
                direction = direction.normalized;
                direction.z = direction.y;
                direction.y = 0;
                StopSlowMo();

                releaseSpeed = Vector3.Distance(endPoint.transform.position, startPoint.transform.position) / maxDistanceForDrag * maxSpeed;
                rB.velocity = direction * releaseSpeed;

                timeToReachMinSpeedCount = 0;
            }
        }

        //ConstantSpeed();
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

    /*private void ConstantSpeed()
    {
        timeToReachMinSpeedCount += Time.deltaTime;

        Vector3 direction = rB.velocity.normalized;

        rB.velocity = direction * releaseSpeed * ballSpeedCurve.Evaluate(timeToReachMinSpeedCount/timeToReachMinSpeed);
    }*/
}
