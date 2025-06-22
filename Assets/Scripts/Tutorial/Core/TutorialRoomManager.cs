using UnityEngine;
using TMPro;
using UnityEngine.Events;
using System.Collections.Generic;

public class TutorialRoomManager : MonoBehaviour
{
    [Header("Task Settings")]
    [SerializeField] private MonoBehaviour taskBehaviour;
    private ITutorialTask currentTask;

    [Header("UI")]
    [SerializeField] private TaskProgressUI taskUI;

    [Header("Room Elements")]
    [SerializeField] private GameObject exitDoor;

    [Header("Description")]
    [TextArea]
    [SerializeField] private string roomDescription;

    [Header("Monster Settings")]
    [SerializeField] private GameObject monsterPrefab;
    [SerializeField] private Transform[] spawnPoints;
    private List<GameObject> spawnedMonsters = new List<GameObject>();

    void Awake()
    {
        if (taskBehaviour == null)
        {
            Debug.LogError("[Tutorial] taskBehaviour not set.");
            return;
        }

        currentTask = taskBehaviour as ITutorialTask;
        if (currentTask == null)
        {
            Debug.LogError("[Tutorial] Allocate ITutorialTask to taskBehaviour");
        }
    }

    public void ActivateRoom()
    {
        SpawnMonsters();

        if (currentTask == null)
        {
            Debug.LogError("[Tutorial] Task not initialized.");
            return;
        }

        currentTask.Initialize(this);

        if (taskUI != null)
        {
            taskUI.SetTask(currentTask.GetTaskDescription());
        }
        else
        {
            Debug.LogWarning("[Tutorial] Task UI not allocate.");
        }

        currentTask.StartTask();
    }

    private void SpawnMonsters()
    {
        if (monsterPrefab == null || spawnPoints.Length == 0) return;
        
        foreach (Transform spawnPoint in spawnPoints)
        {
            GameObject monster = Instantiate(monsterPrefab, spawnPoint.position, spawnPoint.rotation);
            spawnedMonsters.Add(monster);
        }
    }

    public void UpdateTaskProgress(string progress)
    {
        taskUI?.UpdateProgress(progress);
    }

    public void CompleteTask()
    {
        taskUI?.ShowCompletion();
        exitDoor?.SetActive(false);
        TutorialManager.Instance.OnRoomCompleted();
    }

    public void CleanupRoom()
    {
        foreach (var monster in spawnedMonsters)
        {
            if (monster != null) Destroy(monster);
        }
        spawnedMonsters.Clear();
        currentTask?.Cleanup();
    }
    
    public string GetRoomDescription() => roomDescription;
}