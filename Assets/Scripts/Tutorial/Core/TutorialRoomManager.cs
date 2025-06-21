using UnityEngine;
using TMPro;
using UnityEngine.Events;

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
        currentTask?.Cleanup();
    }
    
    public string GetRoomDescription() => roomDescription;
}