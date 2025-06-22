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
    [SerializeField] private MonsterType monsterType = MonsterType.None;

    [SerializeField] private Transform[] spawnPoints;
    private MonsterFactory monsterFactory = new MonsterFactory();

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
        if (monsterType == MonsterType.None || spawnPoints.Length == 0) return;

        foreach (Transform spawnPoint in spawnPoints)
        {
            Vector3 spawnPos = spawnPoint.transform.position;
            Quaternion spawnRot = spawnPoint.transform.rotation;
            GameObject monster = monsterFactory.CreateMonster(monsterType, 1, spawnPos, spawnRot, this.transform);
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
        monsterFactory.DestroyMonsters();
        currentTask?.Cleanup();
    }
    
    public string GetRoomDescription() => roomDescription;
}