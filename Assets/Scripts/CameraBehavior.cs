using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    public Transform ballPlayer;
    public float authorizedGap = 2f, timeToReachMyBall = 0.5f, waitingTimeDeath = 1f;
    public bool toggleOldCam = false;

    public bool recentering = false, switchLevel = false;

    private float waitingTimeDeathCount;

    public void Awake()
    {
       // Screen.SetResolution(100, 1080, false);
    }


    private void Start()
    {
        Vector3 newPos = new Vector3(GameManager.instance.startPoint[GameManager.instance.currentLevel].position.x, transform.position.y, GameManager.instance.startPoint[GameManager.instance.currentLevel].position.z);
        transform.position = newPos;
    }

    void LateUpdate()
    {
        if (ballPlayer == null)
            if (GameObject.FindGameObjectWithTag("Player") != null)
                ballPlayer = GameObject.FindGameObjectWithTag("Player").transform;
            else
                return;

        float distance = transform.position.z - ballPlayer.position.z;

        if (distance < 1) recentering = false;
        if (!switchLevel)
            if (!recentering)
            {
                waitingTimeDeathCount = 0;
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
                    if (distance < 0 && transform.position.z < GameManager.instance.endPosition[GameManager.instance.currentLevel].position.z)
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
        else
        {
            if (waitingTimeDeathCount > waitingTimeDeath)
                SwitchLevel();
            waitingTimeDeathCount += Time.deltaTime * 1 / Time.timeScale;
        }
    }

    public void RecenterCamera()
    {
        if (waitingTimeDeathCount > waitingTimeDeath)
        {
            float zMovement = Mathf.Lerp(transform.position.z, GameManager.instance.startPoint[GameManager.instance.currentLevel].position.z, (1 / timeToReachMyBall * 2) * Time.deltaTime * 1 / Time.timeScale);

            transform.position = new Vector3(transform.position.x, transform.position.y, zMovement);
        }
        waitingTimeDeathCount += Time.deltaTime * 1/Time.timeScale;
    }

    public void SwitchLevel()
    {
        Debug.Log("Switching");
        Vector3 newPos = new Vector3(GameManager.instance.startPoint[GameManager.instance.currentLevel].position.x, transform.position.y, GameManager.instance.startPoint[GameManager.instance.currentLevel].position.z);
        transform.position = newPos;
        switchLevel = false;
        GameManager.instance.SetCurrentLevel();
    }
}
