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
    public float maxSpeed, minSpeed, speedPercentLost = 0.95f, forceRotation;
    public float maxDragScreen, maxArrowScale;

    private float timeToSlowMoCount, timeToReachMinSpeedCount, maxDistanceForDrag;
    private Rigidbody rB;
    private Image startPoint, endPoint;
    private float releaseSpeed, minArrowScale;
    private SpriteRenderer arrowRd;
    private Vector3 oldPos;

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
                arrowScale.y = Vector3.Distance(endPoint.transform.position, startPoint.transform.position) / maxDistanceForDrag * (maxArrowScale - minArrowScale)
                    + ((Vector3.Distance(endPoint.transform.position, startPoint.transform.position)==0)? 0 : minArrowScale);
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
    }

    private void LateUpdate()
    {
        oldPos = transform.position;
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
        if (oldPos != null)
            transform.position = oldPos;

        Vector3 direction = rB.velocity;
        direction.y = 0f;

        Vector3 normal = collision.GetContact(0).normal;
        normal.y = 0;

        Vector3 newDirection = Vector3.Reflect(direction, normal);
        newDirection.y = 0;

        rB.velocity = newDirection;

        Debug.Log(newDirection.x + " " + newDirection.y + " " + newDirection.z);


        /*if (rB.velocity.magnitude > minSpeed)
        {
            rB.velocity *= speedPercentLost;

            if (rB.velocity.magnitude < minSpeed)
            {
                rB.velocity = rB.velocity.normalized * minSpeed;
            }
        }*/
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + rB.velocity);
    }
}
