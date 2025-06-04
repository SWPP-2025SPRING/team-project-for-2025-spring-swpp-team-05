using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceHazard : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerControl player = other.GetComponent<PlayerControl>();
            if (player != null)
            {
                player.EnterIceZone();
                Debug.Log("Ice In");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerControl player = other.GetComponent<PlayerControl>();
            if (player != null)
            {
                player.ExitIceZone();
                Debug.Log("Ice Out");
            }
        }
    }
}
