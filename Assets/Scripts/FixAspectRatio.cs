using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixAspectRatio : MonoBehaviour
{
    private readonly float targetAspect = 16f / 9f;

    void Start()
    {
        ApplyAspectRatio();
    }

    void ApplyAspectRatio()
    {
        float windowAspect = (float)Screen.width / Screen.height;
        float scaleHeight = windowAspect / targetAspect;

        Camera camera = Camera.main;

        if (scaleHeight < 1.0f)
        {
            // 화면이 더 좁을 때 → 상하에 Letterbox (검은 여백)
            Rect rect = camera.rect;

            rect.width = 1.0f;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1.0f - scaleHeight) / 2.0f;

            camera.rect = rect;
        }
        else
        {
            // 화면이 더 넓을 때 → 좌우에 Pillarbox (검은 여백)
            float scaleWidth = 1.0f / scaleHeight;

            Rect rect = camera.rect;

            rect.width = scaleWidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scaleWidth) / 2.0f;
            rect.y = 0;

            camera.rect = rect;
        }
    }
}
