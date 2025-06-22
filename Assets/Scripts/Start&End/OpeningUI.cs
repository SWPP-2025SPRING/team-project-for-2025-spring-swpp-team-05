using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    // Start is called before the first frame update
    void Start()
    {
        origScale = titleText.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime * speed;
        float scale = Mathf.Lerp(minScale, maxScale, Mathf.PingPong(t, 1f));
        titleText.transform.localScale = origScale * scale;
        if (t >= 9f && t <= 12f)
        {
            startButton.transform.Translate(Vector3.up * 2);
            tutorialButton.transform.Translate(Vector3.up * 2);
        }
    }
}
