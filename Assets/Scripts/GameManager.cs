using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance { get; private set; }
    public PlayerControl playerControl;
    public UIManager uiManager;
    // Game Stat
    private float elapsedTime = 0f;

    public bool isGameActive;
    // Start is called before the first frame update

    void Awake()
    {
        // 싱글톤 중복 방지
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // 중복 방지
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // 씬 변경 시 유지하고 싶을 경우

        isGameActive = true;
    }

    void Start()
    {
        isGameActive = true;

    }

    // Update is called once per frame
    void Update()
    {
        if (isGameActive)
        {
            elapsedTime += Time.deltaTime;

            float speed = PlayerStatus.instance.moveSpeed * 3.6f;

            uiManager.UpdateTimer(elapsedTime);
        }
    }

    public void StopTimeTick()
    {
        isGameActive = false;
        Time.timeScale = 0f; // Stop the game time
    }

    public void ResumeTimeTick()
    {
        isGameActive = true;
        Time.timeScale = 1f; // Resume the game time
    }
}