using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInput : MonoBehaviour
{
    public GameObject arrow, pivotArrow;
    public AnimationCurve slowMoCurve, ballSpeedCurve;
    public Gradient gradient;
    public float timeToSlowMo, timeToReachMinSpeed;
    public float maxSpeed, minSpeed, forceRotation;
    public float maxDragScreen, maxArrowScale;

    private float timeToSlowMoCount, timeToReachMinSpeedCount, maxDistanceForDrag;
    private Rigidbody rB;
    private Image startPoint, endPoint;
    private float releaseSpeed, minArrowScale;
    private SpriteRenderer arrowRd;

    // Start is called before the first frame update
    void Start()
    {
        Screen.SetResolution(1080, 2400, true);
        rB = GetComponent<Rigidbody>();

        startPoint = GameObject.Find("StartPoint").GetComponent<Image>();
        endPoint = GameObject.Find("EndPoint").GetComponent<Image>();
        arrowRd = arrow.GetComponent<SpriteRenderer>();

        minArrowScale = minSpeed * maxArrowScale / maxSpeed;

        startPoint.transform.localScale = Vector3.one * Screen.width / 400f;
        endPoint.transform.localScale = Vector3.one * Screen.width / 400f * 0.6f;

        maxDistanceForDrag = Screen.width * maxDragScreen;
    }

    // Update is called once per frame

    private void FixedUpdate()
    {
        
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                startPoint.enabled = true;
                endPoint.enabled = true;
                arrowRd.enabled = true;
                Vector3 positionStartPoint = Input.GetTouch(0).position;
                startPoint.transform.position = positionStartPoint;
            }

            if (Input.GetTouch(0).phase == TouchPhase.Stationary || Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                Vector3 tmp = Input.GetTouch(0).position;

                if (Vector3.Distance(tmp, startPoint.transform.position) > maxDistanceForDrag) {
                    endPoint.transform.position = startPoint.transform.position + (tmp - startPoint.transform.position).normalized * maxDistanceForDrag;}
                else { endPoint.transform.position = tmp;}

                Vector3 arrowScale = Vector3.one;
                arrowScale.y = Vector3.Distance(endPoint.transform.position, startPoint.transform.position) / maxDistanceForDrag * (maxArrowScale - minArrowScale) + minArrowScale;
                arrow.transform.localScale = arrowScale;
                arrowRd.color = gradient.Evaluate((arrowScale.y - minArrowScale) / (maxArrowScale - minArrowScale));

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
                startPoint.enabled = false;
                endPoint.enabled = false;
                arrowRd.enabled = false;

                Vector3 direction = startPoint.transform.position - endPoint.transform.position;

                if (direction != Vector3.zero)
                {
                    direction = direction.normalized;
                    direction.z = direction.y;
                    direction.y = 0;
                    

                    releaseSpeed = Vector3.Distance(endPoint.transform.position, startPoint.transform.position) / maxDistanceForDrag * (maxSpeed - minSpeed) + minSpeed;
                    rB.velocity = direction * releaseSpeed;

                    timeToReachMinSpeedCount = 0;
                }
                StopSlowMo();
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
