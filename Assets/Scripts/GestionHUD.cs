using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GestionHUD : MonoBehaviour
{
    public Text currentScore, highScore;
    public Text collectibleText;
    public FadeInOut arrow, drag, icon;

    public FadeInOut logo;


    // Start is called before the first frame update
    void Start()
    {
        collectibleText.text = "0";
        currentScore.text = "0m";
        highScore.text = "YOUR BEST : 100m";
    }

    // Update is called once per frame
    void Update()
    {
        SetCollectibleText();
        SetCurrentScore();

        switch (GameManager.instance.stateGame)
        {
            case GameManager.statesGame.start:
                break;

            case GameManager.statesGame.tuto:
                logo.Fade(false);
                arrow.Fade(false);
                drag.Fade(false);
                icon.Fade(false);
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
    }
}
