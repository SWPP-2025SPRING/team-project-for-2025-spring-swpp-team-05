using UnityEngine;
using UnityEngine.UI; 
using TMPro; 
using System.Collections; 

public class PETask : MonoBehaviour, ITutorialTask
{
    [Header("References")]
    private TutorialRoomManager roomManager;

    [Header("Task Settings")]
    private int hitCount = 0;
    [SerializeField] private int requiredHits = 1;

    public void Initialize(TutorialRoomManager manager) => roomManager = manager;

    public void StartTask()
    {
        hitCount = 0;
        roomManager?.UpdateTaskProgress($"공에 맞은 횟수: {hitCount}/{requiredHits}");
    }

    public void Cleanup()
    {

    }

    public string GetTaskDescription() => "PE 몬스터가 던진 공에 한 번 맞아 보세요!";

    public void NotifyProjectileHit()
    {
        hitCount++;
        roomManager?.UpdateTaskProgress($"공에 맞은 횟수: {hitCount}/{requiredHits}");

        if (hitCount >= requiredHits && roomManager != null)
        {
            roomManager.CompleteTask();
        }
    }
}
