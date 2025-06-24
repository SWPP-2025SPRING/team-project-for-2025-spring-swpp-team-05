using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public Button pauseButton;
    public GameObject mainUI;
    public GameObject menuUI;
    public Button resumeButton;
    public Button restartButton;
    public Button exitButton;
    // Start is called before the first frame update
    void Start()
    {
        mainUI.SetActive(true);
        menuUI.SetActive(false);

        pauseButton.onClick.AddListener(ToggleMenu);
        resumeButton.onClick.AddListener(ToggleMenu);
        restartButton.onClick.AddListener(() =>
        {
            GameManager.Instance.ResumeTimeTick(); // 게임 시간 재개
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // 현재 씬 재시작
        });
        exitButton.onClick.AddListener(() =>
        {
            GameManager.Instance.ResumeTimeTick(); // 게임 시간 재개
            SceneManager.LoadScene(0);
        });
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMenu();
        }
    }

    public void ToggleMenu()
    {
        bool isActive = mainUI.activeSelf;
        mainUI.SetActive(!isActive);
        menuUI.SetActive(isActive);
        if (isActive)
        {
            GameManager.Instance.StopTimeTick(); // 게임 일시 정지
            BGMManager.Instance.PauseBGM(); // BGM 일시 정지
        }
        else
        {
            GameManager.Instance.ResumeTimeTick(); // 게임 재개
            BGMManager.Instance.ResumeBGM(); // BGM 재개
        }
    }
}
