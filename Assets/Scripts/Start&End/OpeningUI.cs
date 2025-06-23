
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OpeningUI : MonoBehaviour
{
    public GameObject titleText;
    public Button startButton;
    public Button tutorialButton;
    public float speed = 0.5f;
    public float minScale = 0.8f;
    public float maxScale = 1.2f;

    private Vector3 origScale;
    private float t = 0f;

    private RectTransform startRect;
    private RectTransform tutorialRect;
    private Vector2 startOrigin;
    private Vector2 tutorialOrigin;

    private bool moveToCenter = false;

    void Start()
    {
        origScale = titleText.transform.localScale;

        startRect = startButton.GetComponent<RectTransform>();
        tutorialRect = tutorialButton.GetComponent<RectTransform>();

        startOrigin = startRect.anchoredPosition;
        tutorialOrigin = tutorialRect.anchoredPosition;

        startButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("MainScene", LoadSceneMode.Single);
        });
        tutorialButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("TutorialScene", LoadSceneMode.Single);
        });
    }

    void Update()
    {
        t += Time.deltaTime * speed;

        // 타이틀 펄스 효과
        float scale = Mathf.Lerp(minScale, maxScale, Mathf.PingPong(t, 1f));
        titleText.transform.localScale = origScale * scale;

        // 특정 시간 이후 중앙으로 이동 시작
        if (t >= 9f && t <= 12f)
        {
            moveToCenter = true;
        }

        if (moveToCenter)
        {
            Vector2 center = Vector2.zero;

            startRect.anchoredPosition = Vector2.Lerp(startOrigin, center + new Vector2(0, -100), (t - 9f) / 3f);
            tutorialRect.anchoredPosition = Vector2.Lerp(tutorialOrigin, center + new Vector2(0, -250), (t - 9f) / 3f + 0.2f); // 살짝 시간 차이
        }
    }
}
