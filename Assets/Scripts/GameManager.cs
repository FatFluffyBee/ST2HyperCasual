using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FMODUnity;

public class GameManager : MonoBehaviour
{
    public enum statesGame {start, tuto, end, current}
    public statesGame stateGame = statesGame.start;

    public static GameManager instance;

    public void Awake()
    {
        instance = this;  
    }   

    public GameObject playerBall, fragmentedPlayerBall, screenHighScore, screenGameOver, levelDisplay, prefabConso, prefabBreakable;
    public List<Transform> startPoint = new List<Transform>();
    public List<Transform> endPosition = new List<Transform>();
    public HealthBar progressionBar;
    public int currentLevel = 0;
    public PlayerInput playerCurrentBall;

    public float highScoreCount, currentScore;
    public float collectibleCount = 0;
    public bool musicIntense = false;


    private List<Vector3> transConso = new List<Vector3>();
    private List<Vector3> transBreakable = new List<Vector3>();

    FMOD.Studio.EventInstance music;


    // Start is called before the first frame update
    void Start()
    {
        SpawnPlayer();
        SetCurrentLevel();

        music = FMODUnity.RuntimeManager.CreateInstance("event:/Music/Music");
        music.start();
        StoreCoordinates();
    }

    // Update is called once per frame
    void Update()
    {
        if (!musicIntense)
            if (Input.touchCount > 0)
            if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                music.setParameterByName("Start", 1);
                musicIntense = true;
            }

        if (playerCurrentBall == null)
            SpawnPlayer();

        switch(stateGame)
        {
            case statesGame.start:
                if (Input.touchCount > 0)
                    if (Input.GetTouch(0).phase == TouchPhase.Began) stateGame = statesGame.tuto;
                break;

            case statesGame.tuto:
                screenGameOver.SetActive(false);
                screenHighScore.SetActive(false);
                break;

            case statesGame.current:
                break;

            case statesGame.end:
                
                break;

        }
        SetCurrentScore();
        SetLevelProgression();
        ChechDuplicata();
    }

    public void KillPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Vector3 playerPos = player.transform.position;
            Destroy(player);

            Instantiate(fragmentedPlayerBall, playerPos, Quaternion.identity);

            Camera.main.GetComponent<CameraBehavior>().recentering = true;
            //feedback visuel et possible arret 0.5sec
            SpawnPlayer();
            stateGame = statesGame.start;

            SetScreenGameOver();
            FMODUnity.RuntimeManager.PlayOneShot("event:/Ball/Bl_Death/Bl_Death");

            ResetLevel();
        }
    }

    public void SpawnPlayer()
    {
        GameObject instance = Instantiate(playerBall, startPoint[currentLevel].position, Quaternion.identity);
        playerCurrentBall = instance.GetComponent<PlayerInput>();
    }

    public void AddCollectible()
    {
        collectibleCount++;
    }

    public void SetCurrentScore()
    {
        currentScore = Mathf.RoundToInt(playerCurrentBall.transform.position.z - startPoint[currentLevel].transform.position.z);
        if (currentScore < 0) currentScore = 0;
    }

    public void SetScreenHighScore()
    {
        screenHighScore.SetActive(true);
        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/UI_ScoreHigh/UI_ScoreHigh");
    }

    public void SetScreenGameOver()
    {
        screenGameOver.SetActive(true);
        screenGameOver.transform.GetChild(0).GetComponent<Text>().text = currentScore.ToString() + "m";
    }

    public void EndLevel()
    {
        if (playerCurrentBall != null)
            Destroy(playerCurrentBall.gameObject);

        currentLevel++;

        if (currentLevel > 2)
            currentLevel = 0;
        SpawnPlayer();
        Camera.main.GetComponent<CameraBehavior>().switchLevel = true;

        SetScreenHighScore();

        stateGame = statesGame.start;
    }

    public void SetCurrentLevel()
    {
        levelDisplay.GetComponent<Text>().text = ("LEVEL " + (currentLevel + 1).ToString());
    }

    public void SetLevelProgression()
    {
        if (playerCurrentBall != null)
        {
            float progression = (playerCurrentBall.transform.position.z - startPoint[currentLevel].position.z) / (endPosition[currentLevel].position.z - startPoint[currentLevel].position.z);
            progressionBar.SetHealth(progression);
        }
    }

    public void StoreCoordinates()
    {
        GameObject[] consos = GameObject.FindGameObjectsWithTag("Conso");
        GameObject[] breaks = GameObject.FindGameObjectsWithTag("Breakable");

        foreach(GameObject e in consos)
        {
            transConso.Add(e.transform.position);
        }

        foreach(GameObject e in breaks)
        {
            transBreakable.Add(e.transform.position);
        }
    }

    public void ResetLevel()
    {
        GameObject[] consos = GameObject.FindGameObjectsWithTag("Conso");
        GameObject[] breaks = GameObject.FindGameObjectsWithTag("Breakable");

        foreach (GameObject e in consos)
        {
            if (e != null)
                Destroy(e.gameObject);
        }
        foreach (GameObject e in breaks)
        {
            if (e != null)
                Destroy(e.gameObject);
        }

        foreach(Vector3 e in transConso)
        {
            Debug.Log("Conso");
            Instantiate(prefabConso, e, Quaternion.identity);
        }

        foreach(Vector3 e in transBreakable)
        {
            Debug.Log("Break");
            Instantiate(prefabBreakable, e, Quaternion.identity);
        }
    }

    public void ChechDuplicata()
    {
        if (playerCurrentBall != null)
        {
            GameObject[] tmp = GameObject.FindGameObjectsWithTag("Player");

            if (tmp.Length > 1)
            {
                Destroy(tmp[0].gameObject);
                Debug.Log("Ouf");
            }
        }
    }
}
