using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum FlashPreset
{
    // 아주 빠른 깜빡임: FadeIn 0.1초, Hold 0.1초, FadeOut 0.1초
    QuickBlink,

    // 기본 번쩍임: FadeIn 0.25초, Hold 0.5초, FadeOut 0.25초
    StandardFlash,

    // 부드러운 페이드: FadeIn 0.5초, Hold 0.2초, FadeOut 0.5초
    SoftFade,

    // 파도치는 펄스: FadeIn 0.3초, Hold 0.7초, FadeOut 0.3초
    Pulse,

    // 드라마틱 페이드: FadeIn 1.0초, Hold 1.0초, FadeOut 1.0초
    Dramatic
}


public class TitleManager : MonoBehaviour
{
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI subtitleText;
    public TextMeshProUGUI eventText;

    public static TitleManager Instance { get; private set; }
    private void Awake()
    {
        // 싱글톤 중복 방지
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // 중복 방지
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // 씬 변경 시 유지하고 싶을 경우
    }

    void Start()
    {
        titleText.alpha = 0f; // Start with text invisible
        subtitleText.alpha = 0f; // Start with text invisible
        eventText.alpha = 0f; // Start with text invisible
    }

    public void ShowTitle(string title, Color color, FlashPreset preset = FlashPreset.StandardFlash)
    {
        titleText.text = title;
        float duration1, duration2, duration3;
        GetDurations(preset, out duration1, out duration2, out duration3);
        StartCoroutine(FlashText(titleText, color, duration1, duration2, duration3));
    }

    public void ShowSubtitle(string subtitle, Color color, FlashPreset preset = FlashPreset.StandardFlash)
    {
        subtitleText.text = subtitle;
        float duration1, duration2, duration3;
        GetDurations(preset, out duration1, out duration2, out duration3);
        StartCoroutine(FlashText(subtitleText, color, duration1, duration2, duration3));
    }

    public void ShowEventText(string eventTextContent, Color color, FlashPreset preset = FlashPreset.StandardFlash)
    {
        eventText.text = eventTextContent;
        float duration1, duration2, duration3;
        GetDurations(preset, out duration1, out duration2, out duration3);
        StartCoroutine(FlashText(eventText, color, duration1, duration2, duration3));
    }

    private IEnumerator FlashText(TextMeshProUGUI text, Color color, float duration1, float duration2, float duration3)
    {
        Color originalColor = text.color;
        float elapsedTime = 0f;
        text.alpha = 0f; // Start with text invisible
        text.color = color; // Set the text color to the specified color

        while (elapsedTime < duration1)
        {
            elapsedTime += Time.unscaledDeltaTime;
            text.alpha = Mathf.Lerp(0f, 1f, elapsedTime / duration1);
            yield return null;
        }
        // Hold the text visible for duration2
        yield return new WaitForSeconds(duration2);
        elapsedTime = 0f;
        while (elapsedTime < duration2)
        {
            elapsedTime += Time.unscaledDeltaTime;
            text.alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration3);
            yield return null;
        }
    }

    public static void GetDurations(FlashPreset preset, out float duration1, out float duration2, out float duration3)
    {
        switch (preset)
        {
            case FlashPreset.QuickBlink:
                duration1 = 0.1f; duration2 = 0.1f; duration3 = 0.1f;
                break;
            case FlashPreset.StandardFlash:
                duration1 = 0.25f; duration2 = 0.5f; duration3 = 0.25f;
                break;
            case FlashPreset.SoftFade:
                duration1 = 0.5f; duration2 = 0.2f; duration3 = 0.5f;
                break;
            case FlashPreset.Pulse:
                duration1 = 0.3f; duration2 = 0.7f; duration3 = 0.3f;
                break;
            case FlashPreset.Dramatic:
                duration1 = 1.0f; duration2 = 1.0f; duration3 = 1.0f;
                break;
            default:
                // fallback to StandardFlash
                duration1 = 0.25f; duration2 = 0.5f; duration3 = 0.25f;
                break;
        }
    }
}
