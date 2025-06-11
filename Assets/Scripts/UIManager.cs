using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("UI Texts")]
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI timerText;

    // TODO: 공격 스탯 필요없다는 결론 나오면 다 삭제
    [Header("Stat Bars")]
    public Image atkBar;
    public Image rangeBar;

    [Header("Speed Feedback")]
    private float lastSpeed;
    private readonly float defaultSpeed = 36.0f;
    private Color defaultColor = Color.white;
    private Coroutine colorCoroutine;
    public float transitionDuration = 0.5f;

    // light green
    private static readonly Color SpeedUpColor = new Color(0.6f, 1f, 0.5f);
    // light pink
    private static readonly Color SpeedDownColor = new Color(1f, 0.6f, 0.6f);

    void Start()
    {
        lastSpeed = defaultSpeed;
    }

    // 1. Timer Update
    public void UpdateTimer(float elapsedTime)
    {
        timerText.text = FormatElapsedTime(elapsedTime);
    }

    string FormatElapsedTime(float time)
    {
        int minutes = (int)(time / 60);
        float seconds = time % 60;

        if (minutes > 0)
            return $"{minutes}:{seconds:00.00}";
        else
            return $"00:{seconds:00.00}";
    }

    // 2. Speed Update (+Color)
    public void UpdateSpeed(float speed)
    {
        speedText.text = $"<size=96>{speed:F1}</size><size=40> km/h</size>";

        if (speed > lastSpeed)
        {
            StartColorTransition(SpeedUpColor);
        }
        else if (speed < lastSpeed)
        {
            StartColorTransition(SpeedDownColor);
        }

        lastSpeed = speed;
    }

    void StartColorTransition(Color targetColor)
    {
        if (colorCoroutine != null) StopCoroutine(colorCoroutine);
        colorCoroutine = StartCoroutine(ColorLerpCoroutine(targetColor));
    }

    IEnumerator ColorLerpCoroutine(Color targetColor)
    {
        float t = 0f;
        Color startColor = defaultColor;

        while (t < transitionDuration)
        {
            speedText.color = Color.Lerp(startColor, targetColor, t / transitionDuration);
            t += Time.deltaTime;
            yield return null;
        }

        speedText.color = targetColor;

        yield return new WaitForSeconds(0.2f);

        t = 0f;
        while (t < transitionDuration)
        {
            speedText.color = Color.Lerp(targetColor, defaultColor, t / transitionDuration);
            t += Time.deltaTime;
            yield return null;
        }

        speedText.color = defaultColor;
    }

    // Player ATK / RANGE Update
    // TODO: 공격 스탯 필요없다는 결론 나오면 다 삭제
    public void UpdateStats(float speed, float atkRatio, float rangeRatio)
    {
        UpdateSpeed(speed);

        if (atkBar != null)
            atkBar.fillAmount = atkRatio;

        if (rangeBar != null)
            rangeBar.fillAmount = rangeRatio;
    }
}

