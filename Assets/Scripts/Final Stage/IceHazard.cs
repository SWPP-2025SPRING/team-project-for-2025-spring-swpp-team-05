using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceHazard : MonoBehaviour
{
    public AudioClip iceCollisionSound; // 얼음 장애물과 충돌 시 재생할 소리
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SoundEffectManager.Instance.PlayOneShotOnce(iceCollisionSound); // 얼음 장애물과 충돌 시 소리 재생
            PlayerControl player = other.GetComponent<PlayerControl>();
            if (player != null)
            {
                player.EnterIceZone();
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
            }
        }
    }
}
