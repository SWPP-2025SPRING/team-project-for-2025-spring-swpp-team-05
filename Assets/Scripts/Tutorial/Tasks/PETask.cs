using UnityEngine;
using UnityEngine.UI; 
using TMPro; 
using System.Collections; 

public class PETask : MonoBehaviour, ITutorialTask
{
    private TutorialRoomManager roomManager;

    public void Initialize(TutorialRoomManager manager) => roomManager = manager;

    public void StartTask()
    {
        // 몬스터 관련 초기화
    }

    public void Cleanup()
    {
        // 정리 작업
    }

    public string GetTaskDescription() => "던지는 공과 부딛혀 보세요!";

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && roomManager != null)
        {
            roomManager.CompleteTask();
        }
    }
}
