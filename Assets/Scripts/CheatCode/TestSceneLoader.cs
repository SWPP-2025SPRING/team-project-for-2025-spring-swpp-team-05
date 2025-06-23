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

    public Button[] sceneChangeButton;
    public string[] sceneNames;

    void Start()
    {
        // 일단 모든 버튼 비활성화
        for (int i = 0; i < sceneChangeButton.Length; i++)
        {
            int index = i; // 클로저 방지
            sceneChangeButton[i].gameObject.SetActive(false);
            sceneChangeButton[i].interactable = false;

            // 미리 이벤트 연결해두고, 치트 모드에서만 활성화되게 함
            sceneChangeButton[i].onClick.AddListener(() =>
            {
                LoadScene(sceneNames[index]);
            });
        }
    }

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
                pressCount = 1;
            }

            lastPressTime = currentTime;

            if (pressCount >= requiredPresses)
            {
                ActivateCheatMode();
                pressCount = 0;
            }
        }
    }

    void ActivateCheatMode()
    {
        if (!cheatModeActive)
        {
            cheatModeActive = true;
            Debug.Log("🎉 Cheat Mode Activated!");
        }

        // 버튼 전부 활성화
        for (int i = 0; i < sceneChangeButton.Length; i++)
        {
            sceneChangeButton[i].gameObject.SetActive(true);
            sceneChangeButton[i].interactable = true;
        }
    }

    void LoadScene(string sceneName)
    {
        if (cheatModeActive)
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.Log("Cheat mode is not active. Cannot load scene.");
        }
    }
}