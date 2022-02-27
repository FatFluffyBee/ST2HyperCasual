using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GestionHUD : MonoBehaviour
{
    public Text currentScore, highScore;
    public Text collectibleText;

    public GameObject tutoHolder, logo;


    // Start is called before the first frame update
    void Start()
    {
        collectibleText.text = "0";
        currentScore.text = "0m";
        highScore.text = "YOUR BEST : 100m";
        logo.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        SetCollectibleText();
        SetCurrentScore();

        switch (GameManager.instance.stateGame)
        {
            case GameManager.statesGame.start:
                tutoHolder.SetActive(true);
                break;

            case GameManager.statesGame.tuto:
                tutoHolder.SetActive(false);
                logo.SetActive(false);
                break;

            case GameManager.statesGame.current:
                break;

            case GameManager.statesGame.end:
                break;
        }
    }

    public void SetCollectibleText()
    {
        collectibleText.text = GameManager.instance.collectibleCount.ToString();
    }

    public void SetCurrentScore()
    {
        currentScore.text = GameManager.instance.currentScore + "m";
        highScore.text = GameManager.instance.highScoreCount + "m";
    }
}
