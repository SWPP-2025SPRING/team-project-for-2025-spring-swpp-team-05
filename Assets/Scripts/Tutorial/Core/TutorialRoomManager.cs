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

    public UnityEvent onRoomComplete;

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
            Debug.LogWarning("[Tutorial] Task UI not allocated.");
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
        exitDoor?.SetActive(true);
        onRoomComplete?.Invoke();
    }

    public void CleanupRoom()
    {
        currentTask?.Cleanup();
    }
}