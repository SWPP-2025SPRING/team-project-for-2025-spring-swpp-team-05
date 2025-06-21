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
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI levelUpText;


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

    [Header("Level UI Feedback")]
    public float flashDuration = 0.1f;
    public float fadeDuration = 1.0f;
    public Color levelUpColor = new Color(1f, 0.8f, 0.2f); // light yellow

    void Start()
    {
        lastSpeed = defaultSpeed;
        levelUpText.alpha = 0f; // Start with level up text hidden
    }

    public void UpdateLevel(int level)
    {
        levelText.text = $"Lv: {level}";
        levelUpText.text = $"+{level}";

        if (levelUpText.alpha > 0f)
        {
            StopCoroutine(FlashLevelUpText());
        }

        StartCoroutine(FlashLevelUpText());
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

    IEnumerator FlashLevelUpText()
    {
        levelText.color = levelUpColor;
        levelUpText.alpha = 1f;
        yield return new WaitForSeconds(flashDuration);

        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            levelUpText.alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            levelText.color = Color.Lerp(levelUpColor, defaultColor, elapsed / fadeDuration);
            yield return null;
        }

        Debug.Log("Level up text faded out.");
        levelUpText.alpha = 0f;
        levelText.color = defaultColor; // Reset color after fading out
    }

    // Player ATK / RANGE Update
    public void UpdateStats(float speed)
    {
        UpdateSpeed(speed);

    }
}

