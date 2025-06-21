using UnityEngine;
using System;
using System.Collections;

public class ProfessorTask : MonoBehaviour, ITutorialTask
{
    [Header("References")]
    [SerializeField] private ProfessorView professorView;
    
    [Header("Task Settings")]
    [SerializeField] private readonly float requiredTime = 3f;
    
    private TutorialRoomManager roomManager;
    private Coroutine sightTimerCoroutine;
    private float currentTime = 0f;
    private bool isActive = false;

    void Start()
    {
        if (professorView == null)
        {
            professorView = GetComponent<ProfessorView>();
            if (professorView == null)
            {
                Debug.LogError("[Tutorial] ProfessorView component not found!");
                return;
            }
        }
        
        professorView.OnPlayerEnterSight += HandlePlayerEnterSight;
        professorView.OnPlayerExitSight += HandlePlayerExitSight;
    }

    void OnDestroy()
    {
        if (professorView != null)
        {
            professorView.OnPlayerEnterSight -= HandlePlayerEnterSight;
            professorView.OnPlayerExitSight -= HandlePlayerExitSight;
        }
    }

    public void Initialize(TutorialRoomManager manager) => roomManager = manager;

    public void StartTask()
    {
        isActive = true;
        currentTime = 0f;
        roomManager?.UpdateTaskProgress($"{currentTime:F1}/{requiredTime}초");
    }

    public void Cleanup()
    {
        isActive = false;
        if (sightTimerCoroutine != null)
        {
            StopCoroutine(sightTimerCoroutine);
            sightTimerCoroutine = null;
        }
    }

    public string GetTaskDescription() => "교수님의 시야 안에 3초간 머무르세요!";

    private void HandlePlayerEnterSight()
    {
        if (!isActive) return;
        
        if (sightTimerCoroutine == null)
        {
            sightTimerCoroutine = StartCoroutine(SightTimer());
        }
    }

    private void HandlePlayerExitSight()
    {
        if (sightTimerCoroutine != null)
        {
            StopCoroutine(sightTimerCoroutine);
            sightTimerCoroutine = null;
        }
    }

    private IEnumerator SightTimer()
    {
        currentTime = 0f;
        
        while (currentTime < requiredTime && isActive)
        {
            currentTime += Time.deltaTime;
            roomManager?.UpdateTaskProgress($"{currentTime:F1}/{requiredTime}초");
            yield return null;
        }

        if (currentTime >= requiredTime && isActive)
        {
            roomManager?.CompleteTask();
        }
        
        sightTimerCoroutine = null;
    }
}
