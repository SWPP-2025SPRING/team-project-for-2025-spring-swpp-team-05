using UnityEngine;

public class TutorialRoomTrigger : MonoBehaviour
{
    [SerializeField] private TutorialRoomManager roomManager;

    void Start()
    {
        // 자동 할당 (옵션)
        if (roomManager == null)
            roomManager = GetComponentInParent<TutorialRoomManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && roomManager != null)
        {
            TutorialManager.Instance.OnRoomEntered(roomManager);
        }
    }
}
