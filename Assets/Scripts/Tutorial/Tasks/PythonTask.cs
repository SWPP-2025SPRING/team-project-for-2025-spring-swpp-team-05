using UnityEngine;

public class PythonTask : MonoBehaviour, ITutorialTask
{
    [Header("References")]
    private TutorialRoomManager roomManager;

    [Header("Task Settings")]
    private int debuffCount = 0;
    [SerializeField] private int requiredDebuffCount = 2;


    public void Initialize(TutorialRoomManager manager) => roomManager = manager;

    public void StartTask()
    {
        debuffCount = 0;
        UpdateProgress();
    }

    public void Cleanup()
    {

    }

    public string GetTaskDescription() => $"디버프를 {requiredDebuffCount}번 체험하세요!";

    public void NotifyDebuffTriggered()
    {
        debuffCount++;
        UpdateProgress();

        if (debuffCount >= requiredDebuffCount)
        {
            roomManager?.CompleteTask();
        }
    }

    private void UpdateProgress()
    {
        roomManager?.UpdateTaskProgress($"디버프 체험: {debuffCount}/{requiredDebuffCount}회");
    }
}
