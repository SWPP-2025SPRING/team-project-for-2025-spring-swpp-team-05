using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MosquitoHazard : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // PlayerStatus.instance.SlowPlayer(0.3f);
            Debug.Log("🦟 Mosquito!!");

            Camera.main.GetComponent<CameraBlurEffect>()?.TriggerMosquitoBlur();
            DebufManager.Instance.UpdateDebufText(DebufType.Mosquito);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("🦟 Mosquito zone exited");
            DebufManager.Instance.UpdateDebufText(DebufType.None);
        }
    }
}
