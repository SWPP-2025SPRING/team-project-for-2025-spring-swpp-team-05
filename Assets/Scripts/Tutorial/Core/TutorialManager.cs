using System;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private TutorialRoomManager[] rooms;
    private int currentRoomIndex = 0;

    void Start()
    {
        InitializeRooms();
    }

    void InitializeRooms()
    {
        if (rooms == null || rooms.Length == 0)
        {
            Debug.LogError("Rooms array is not assigned!");
            return;
        }

        for (int i = 0; i < rooms.Length; i++)
        {
            if (rooms[i] != null)
            {
                rooms[i].onRoomComplete.AddListener(HandleRoomCompletion);
                rooms[i].gameObject.SetActive(i == 0);
            }
            else
            {
                Debug.LogWarning($"[Tutorial] Room at index {i} is null!");
            }
        }
    }

    void HandleRoomCompletion()
    {
        currentRoomIndex++;

        if (currentRoomIndex >= rooms.Length || rooms[currentRoomIndex] == null)
        {
            Debug.Log("[Tutorial] Tutorial finished!");
            return;
        }

        rooms[currentRoomIndex].gameObject.SetActive(true);
        rooms[currentRoomIndex].ActivateRoom();
    }

    void OnDestroy()
    {
        foreach (var room in rooms)
        {
            room?.onRoomComplete.RemoveListener(HandleRoomCompletion);
        }
    }
}