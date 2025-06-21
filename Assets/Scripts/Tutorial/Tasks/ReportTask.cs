using UnityEngine;

public class ReportTask : MonoBehaviour, ITutorialTask
{
    [Header("References")]
    private TutorialRoomManager roomManager;

    [Header("Task Settings")]
    private int currentKills = 0;
    [SerializeField] private int requiredKills = 2;


    public void Initialize(TutorialRoomManager manager) => roomManager = manager;

    public void StartTask()
    {
        currentKills = 0;
        UpdateProgress();
    }

    public void Cleanup()
    {

    }

    public string GetTaskDescription() => "달려오는 Report 몬스터의 문제를 풀어주세요!";

    public void NotifyMonsterDestroyed()
    {
        currentKills++;
        UpdateProgress();
        
        if (currentKills >= requiredKills)
        {
            roomManager?.CompleteTask();
        }
    }

    private void UpdateProgress()
    {
        roomManager?.UpdateTaskProgress($"해결한 몬스터: {currentKills}/{requiredKills}");
    }
}
