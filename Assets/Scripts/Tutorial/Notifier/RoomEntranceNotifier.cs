using UnityEngine;

public class RoomEntranceNotifier : MonoBehaviour
{
    [SerializeField] private TutorialRoomManager roomManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TutorialManager.Instance.OnRoomEntered(roomManager);
            gameObject.SetActive(false);
        }
    }
}
