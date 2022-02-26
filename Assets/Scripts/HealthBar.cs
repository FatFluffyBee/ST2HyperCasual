using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Image fill;
    private Gradient gradient;

    public void SetHealth(float health)
    {
        slider.value = health;
    }

    public void SethealthAndGradient(float health)
    {
        slider.value = health;
        fill.color = gradient.Evaluate(health);
    }

    public void SetGradient(Gradient gradient)
    {
        this.gradient = gradient;
    }

    public void SetColor(Color color)
    {
        fill.color = color;
    }
}
