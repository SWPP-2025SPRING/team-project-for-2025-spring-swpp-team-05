using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnteranceForward : MonoBehaviour
{
    public enum EnteranceType { Enter, Exit }
    public EnteranceType enteranceType; // Enter or Exit
    private RoomSpawnManager parentManager; // Reference to the RoomSpawnManager
    // Start is called before the first frame update
    void Start()
    {
        parentManager = GetComponentInParent<RoomSpawnManager>();
        if (parentManager == null)
        {
            Debug.LogError("EnteranceForward: Parent RoomSpawnManager not found!");
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (enteranceType == EnteranceType.Enter)
            {
                parentManager.HandleEnter(other);
            }
            else if (enteranceType == EnteranceType.Exit)
            {
                parentManager.HandleExit(other);
            }
        }
    }
}
