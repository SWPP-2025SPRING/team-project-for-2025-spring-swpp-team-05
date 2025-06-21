using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFlipDebuff : IDebuff
{
    public float Duration { get; private set; }

    public CameraFlipDebuff(float duration)
    {
        this.Duration = duration;
    }

    public void Apply(GameObject target)
    {
        Camera cam = Camera.main;
        if (cam != null)
        {
            MonoBehaviour runner = target.GetComponent<MonoBehaviour>();
            if (runner != null)
            {
                runner.StartCoroutine(FlipCameraCoroutine(cam));
            }
        }
    }

    public void Remove(GameObject target)
    {
        // 아무 것도 안 해도 됨 — 코루틴 내에서 원복 처리됨
    }

    private IEnumerator FlipCameraCoroutine(Camera cam)
    {
        var followScript = cam.GetComponent<FollowPlayer>();
        if (followScript != null)
        {
            followScript.SetFlipped(true);
            DebufManager.Instance.UpdateDebufText(DebufType.CameraFlip);
        }

        yield return new WaitForSeconds(Duration);

        if (followScript != null)
        {
            followScript.SetFlipped(false); // 👈 원복
            DebufManager.Instance.UpdateDebufText(DebufType.None);
        }
    }
}
