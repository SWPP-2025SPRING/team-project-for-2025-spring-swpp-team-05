using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnteranceForward : MonoBehaviour
{
    public enum EnteranceType { Enter, Exit }
    public EnteranceType enteranceType; // Enter or Exit
    private RoomSpawnManager parentManager; // Reference to the RoomSpawnManager
    private Collider enteranceCollider; // Reference to the collider of this enterance
    // Start is called before the first frame update
    void Start()
    {
        parentManager = GetComponentInParent<RoomSpawnManager>();
        if (parentManager == null)
        {
            Debug.LogError("EnteranceForward: Parent RoomSpawnManager not found!");
        }
        parentManager.RegisterEnterance(this); // Register this enterance with the parent manager
        enteranceCollider = GetComponent<Collider>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (enteranceType == EnteranceType.Enter)
            {
                enteranceCollider.isTrigger = parentManager.HandleEnter(other);
            }
            else if (enteranceType == EnteranceType.Exit)
            {
                enteranceCollider.isTrigger = parentManager.HandleExit(other);
            }
        }
    }

    public void SetTrigger(bool isTrigger)
    {
        if (enteranceCollider != null)
        {
            enteranceCollider.isTrigger = isTrigger;
        }
    }
}
