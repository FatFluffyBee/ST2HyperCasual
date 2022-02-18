using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInOut : MonoBehaviour
{
    public bool imageIn = true;
    public float timeToFade = 1f;

    private Image image;
    private Text text;
    private float timeCount;
    private bool isFadingIn = true;

    private void Start()
    {
        image = GetComponent<Image>();
    }

    private void Update()
    {
        if (imageIn)
        {
            if (isFadingIn)
            {
                image.color = new Color(255, 255, 255, timeCount / timeToFade);
                timeCount += Time.deltaTime * 1 / Time.timeScale;
            }
            else
            {
                image.color = new Color(255, 255, 255, timeCount / timeToFade);
                timeCount -= Time.deltaTime * 1 / Time.timeScale;
            }
        }
        else
        {
            if (isFadingIn)
            {
                text.color = new Color(255, 255, 255, timeCount / timeToFade);
                timeCount += Time.deltaTime * 1 / Time.timeScale;
            }
            else
            {
                text.color = new Color(255, 255, 255, timeCount / timeToFade);
                timeCount -= Time.deltaTime * 1 / Time.timeScale;
            }


            if (timeCount > timeToFade) timeCount = timeToFade;
            if (timeCount < 0) timeCount = 0;
        }
    }

    public void Fade(bool choice)
    {
        isFadingIn = choice;
    }
}
