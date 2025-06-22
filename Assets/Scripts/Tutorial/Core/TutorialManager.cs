using System;
using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TutorialUI tutorialUI;

    [Header("Room Settings")]
    [SerializeField] private TutorialRoomManager[] rooms;

    [Header("Player Settings")]
    [SerializeField] private GameObject player;

    [Header("Scene Settings")]
    [SerializeField] private string mainSceneName = "MainScene";

    private TutorialRoomManager currentRoom;
    private int currentRoomIndex = 0;
    private bool isPaused = true;
    private bool awaitingRoomStart = false;
    private bool tutorialStarted = false;
    private bool isTutorialComplete = false;

    public static TutorialManager Instance { get; private set; }

    [Header("Delay Settings")]
    [SerializeField] private float startDelay = 1f; // 1초 딜레이

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        StartCoroutine(InitializeWithDelay());
    }

    IEnumerator InitializeWithDelay()
    {
        yield return new WaitForSeconds(startDelay);

        tutorialUI.gameObject.SetActive(true);

        ShowTutorial("왼쪽/A : 왼쪽 이동\n오른쪽/D : 오른쪽 이동\n아래 방향 S: 감속\n\nF 키를 눌러 시작");

        foreach (var room in rooms)
        {
            room.gameObject.SetActive(false);
        }

        PauseGame();
    }

    void Update()
    {
        if (isPaused && Input.GetKeyDown(KeyCode.F))
        {
            if (isTutorialComplete)
            {
                LoadMainScene();
                ResumeGame();
            }
            else if (!tutorialStarted)
            {
                tutorialStarted = true;
                HideTutorial();
                ActivateRoom(rooms[currentRoomIndex]);
                ResumeGame();
            }
            else if (awaitingRoomStart)
            {
                awaitingRoomStart = false;
                HideTutorial();
                currentRoom.ActivateRoom();
                ResumeGame();
            }
        }
    }

    private void ActivateRoom(TutorialRoomManager room)
    {
        currentRoom = room;
        room.gameObject.SetActive(true);
    }

    public void OnRoomEntered(TutorialRoomManager room)
    {
        currentRoom = room;
        PauseGame();
        ShowTutorial(room.GetRoomDescription() + "\n\nF 키를 눌러 계속 진행합니다.");
        awaitingRoomStart = true;
    }

    public void OnRoomCompleted()
    {
        currentRoom.CleanupRoom();
        currentRoom.gameObject.SetActive(false);
        currentRoomIndex++;

        if (currentRoomIndex < rooms.Length)
        {
            ActivateRoom(rooms[currentRoomIndex]);
        }
        else
        {
            isTutorialComplete = true;
            PauseGame();
            ShowTutorial("튜토리얼 완료!\n게임을 즐겨주세요.\n\nF키를 눌러 메인 게임으로 이동합니다.");
        }
    }

    private void LoadMainScene()
    {
        SceneManager.LoadScene(mainSceneName);
    }

    private void PauseGame()
    {
        Time.timeScale = 0.0f;
        isPaused = true;
    }

    private void ResumeGame()
    {
        Time.timeScale = 1.0f;
        isPaused = false;
    }

    private void ShowTutorial(string message)
    {
        tutorialUI.ShowTutorial(message);
        PauseGame();
    }

    private void HideTutorial()
    {
        tutorialUI.HideTutorial();
    }

    public bool IsPaused() => isPaused;
}