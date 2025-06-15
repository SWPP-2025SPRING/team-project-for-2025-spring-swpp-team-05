using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTiltDebuff : IDebuff
{
    public float Duration { get; private set; }
    private float tiltAmount;

    public CameraTiltDebuff(float Duration, float tiltAmount = 15f)
    {
        this.Duration = Duration;
        this.tiltAmount = tiltAmount;
    }

    public void Apply(GameObject target)
    {
        Camera cam = Camera.main;
        if (cam != null)
        {
            MonoBehaviour runner = target.GetComponent<MonoBehaviour>();
            if (runner != null)
            {
                runner.StartCoroutine(TiltCameraCoroutine(cam));
            }
        }
    }

    public void Remove(GameObject target)
    {
        // 아무 것도 하지 않음 (코루틴 안에서 원위치 처리함)
    }

    private IEnumerator TiltCameraCoroutine(Camera cam)
    {
        FollowPlayer follow = cam.GetComponent<FollowPlayer>();
        if (follow == null) yield break;

        float timer = 0f;
        while (timer < Duration)
        {
            float zTilt = Mathf.Sin(Time.time * 10f) * tiltAmount;
            follow.SetTiltAngle(zTilt);
            timer += Time.deltaTime;
            yield return null;
        }

        follow.SetTiltAngle(0f);
    }
}
