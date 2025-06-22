using UnityEngine;

public class ProfessorTask : MonoBehaviour, ITutorialTask
{
    [Header("References")]
    [SerializeField] private Transform goalTransform;

    private TutorialRoomManager roomManager;
    private bool isActive = false;
    private bool taskCompleted = false;

    public void Initialize(TutorialRoomManager manager) => roomManager = manager;

    public void StartTask()
    {
        isActive = true;
        taskCompleted = false;
        roomManager?.UpdateTaskProgress("흰색 문까지 이동하세요!");
    }

    public void Cleanup()
    {
        isActive = false;
    }

    public string GetTaskDescription() => "흰색 문까지 이동하세요!";

    public void NotifyArrivedAtGoal()
    {
        if (!isActive || taskCompleted) return;
        taskCompleted = true;
        roomManager?.CompleteTask();
    }
}
