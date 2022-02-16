using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    public Transform ballPlayer;
    public float authorizedGap = 2f, timeToReachMyBall = 0.5f;

    void Update()
    {
        if (ballPlayer == null)
            ballPlayer = GameObject.FindGameObjectWithTag("Player").transform;

        float distance = transform.position.z - ballPlayer.position.z;

        if (distance < - authorizedGap || distance > authorizedGap)
        {
            float zMovement = Mathf.Lerp(transform.position.z, ballPlayer.position.z,  (1/timeToReachMyBall) * Time.deltaTime * 1/Time.timeScale);

            transform.position = new Vector3(transform.position.x, transform.position.y, zMovement);
        }
    }
}
