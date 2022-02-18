using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public enum statesGame {start, tuto, end, current}
    public statesGame stateGame = statesGame.start;

    public static GameManager instance;

    public void Awake()
    {
        instance = this;  
    }

    public GameObject playerBall;
    private Transform startPoint;
    public PlayerInput playerCurrentBall;

    public float highScoreCount, currentScore;
    public float collectibleCount = 0;


    // Start is called before the first frame update
    void Start()
    {
        startPoint = GameObject.Find("PlayerStartPoint").transform;

        SpawnPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        switch(stateGame)
        {
            case statesGame.start:
                if (Input.touchCount > 0)
                    if (Input.GetTouch(0).phase == TouchPhase.Began) stateGame = statesGame.tuto;
                break;

            case statesGame.tuto:

                break;

            case statesGame.current:
                break;

            case statesGame.end:
                break;

        }
        SetCurrentScore();
    }

    public void KillPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Destroy(player);
        Camera.main.GetComponent<CameraBehavior>().recentering = true;
        //feedback visuel et possible arret 0.5sec
        SpawnPlayer();
    }

    public void SpawnPlayer()
    {
        GameObject instance = Instantiate(playerBall, startPoint.position, Quaternion.identity);
        playerCurrentBall = instance.GetComponent<PlayerInput>();
    }

    public void AddCollectible()
    {
        collectibleCount++;
    }

    public void SetCurrentScore()
    {
        currentScore = Mathf.RoundToInt(playerCurrentBall.transform.position.z - startPoint.transform.position.z);
        if (currentScore < 0) currentScore = 0;
    }
}
