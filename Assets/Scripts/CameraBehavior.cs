using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    public Transform ballPlayer;
    public float authorizedGap = 2f, timeToReachMyBall = 0.5f;
    public bool toggleOldCam = false;

    public bool recentering = false;

    private float zInitialPos;

    private void Start()
    {
        zInitialPos = transform.position.z;
    }

    void LateUpdate()
    {
        if (ballPlayer == null)
            ballPlayer = GameObject.FindGameObjectWithTag("Player").transform;

        float distance = transform.position.z - ballPlayer.position.z;

        if (distance < 1) recentering = false;

        if (!recentering)
        {
            if (toggleOldCam)
            {
                if (distance < -authorizedGap || distance > authorizedGap)
                {
                    float zMovement = Mathf.Lerp(transform.position.z, ballPlayer.position.z, (1 / timeToReachMyBall) * Time.deltaTime * 1 / Time.timeScale);

                    transform.position = new Vector3(transform.position.x, transform.position.y, zMovement);
                }
            }
            else
            {
                if (distance < 0)
                {
                    float zMovement = Mathf.Lerp(transform.position.z, ballPlayer.position.z, (1 / timeToReachMyBall) * Time.deltaTime * 1 / Time.timeScale);

                    transform.position = new Vector3(transform.position.x, transform.position.y, zMovement);
                }
            }
        }
        else
        {
            RecenterCamera();
        }
    }

    public void RecenterCamera()
    {
        float zMovement = Mathf.Lerp(transform.position.z, zInitialPos, (1 / timeToReachMyBall * 2) * Time.deltaTime * 1 / Time.timeScale);

        transform.position = new Vector3(transform.position.x, transform.position.y, zMovement);
    }
}
