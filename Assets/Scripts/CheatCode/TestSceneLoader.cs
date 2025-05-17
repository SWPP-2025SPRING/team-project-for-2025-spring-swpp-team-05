using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TestSceneLoader : MonoBehaviour
{
    public float maxDelay = 0.5f;
    public int requiredPresses = 6;

    private int pressCount = 0;
    private float lastPressTime = 0f;

    public bool cheatModeActive = false;

    public Button sceneChangeButton;

    // Start is called before the first frame update
    void Start()
    {
        sceneChangeButton.onClick.AddListener(LoadTestScene);
        sceneChangeButton.interactable = false; // 버튼을 처음에 비활성화
        sceneChangeButton.gameObject.SetActive(false); // 버튼을 처음에 비활성화
    }



    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            float currentTime = Time.time;

            if (currentTime - lastPressTime <= maxDelay)
            {
                pressCount++;
            }
            else
            {
                pressCount = 1; // 너무 늦게 눌렀으면 카운트 초기화
            }

            lastPressTime = currentTime;

            if (pressCount >= requiredPresses)
            {
                ActivateCheatMode();
                pressCount = 0; // 다시 초기화
            }
        }
    }

    void ActivateCheatMode()
    {
        if (!cheatModeActive)
        {
            cheatModeActive = true;
            Debug.Log("🎉 Cheat Mode Activated!");
            // 여기에 원하는 치트 기능 실행
        }
        sceneChangeButton.gameObject.SetActive(true); // 버튼 활성화
        sceneChangeButton.interactable = true; // 버튼 활성화
    }

    void LoadTestScene()
    {
        if (cheatModeActive)
        {
            SceneManager.LoadScene("TestScene");
        }
        else
        {
            Debug.Log("Cheat mode is not active. Cannot load TestScene.");
        }
    }
}
