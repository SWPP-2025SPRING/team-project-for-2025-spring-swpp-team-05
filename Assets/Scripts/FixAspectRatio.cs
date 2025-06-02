using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixAspectRatio : MonoBehaviour
{
    private readonly float targetAspect = 16f / 9f;
    private float lastScreenWidth;
    private float lastScreenHeight;
    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
        ApplyAspectRatio();
        SaveCurrentScreenSize();
    }

    void Update()
    {
        // 브라우저 창 크기가 바뀌었을 때만 다시 적용
        if (Screen.width != lastScreenWidth || Screen.height != lastScreenHeight)
        {
            ApplyAspectRatio();
            SaveCurrentScreenSize();
        }
    }


    void ApplyAspectRatio()
    {
        float windowAspect = (float)Screen.width / Screen.height;
        float scaleHeight = windowAspect / targetAspect;


        if (scaleHeight < 1.0f)
        {
            // 화면이 좁을 때: 상하 Letterbox
            Rect rect = new Rect(0, (1f - scaleHeight) / 2f, 1f, scaleHeight);
            cam.rect = rect;
        }
        else
        {
            // 화면이 넓을 때: 좌우 Pillarbox
            float scaleWidth = 1f / scaleHeight;
            Rect rect = new Rect((1f - scaleWidth) / 2f, 0, scaleWidth, 1f);
            cam.rect = rect;
        }
    }

    void SaveCurrentScreenSize()
    {
        lastScreenWidth = Screen.width;
        lastScreenHeight = Screen.height;
    }
}
