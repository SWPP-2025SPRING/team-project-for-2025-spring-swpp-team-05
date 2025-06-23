using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MiniMapController : MonoBehaviour
{
    [Header("필수 레퍼런스")]
    public Camera miniMapCam;           // MiniMapCamera
    public RectTransform miniMapRect;   // RawImage의 RectTransform
    public GameObject markerPrefab;     // MarkerDot 프리팹

    [Header("추적할 타겟")]
    public Transform[] worldTargets;    // 플레이어·적·웨이포인트 등

    // 내부 관리용
    private readonly List<RectTransform> markers = new();

    void Start()
    {
        // 타겟 수만큼 마커 생성
        foreach (Transform _ in worldTargets)
        {
            var m = Instantiate(markerPrefab, miniMapRect).GetComponent<RectTransform>();
            markers.Add(m);
        }
    }

    void Update()
    {
        for (int i = 0; i < worldTargets.Length; i++)
        {
            UpdateMarker(i);
        }
    }

    void UpdateMarker(int idx)
    {
        var target = worldTargets[idx];
        var markerRT = markers[idx];

        Vector3 vp = miniMapCam.WorldToViewportPoint(target.position);

        if (vp.z < 0)
        {
            markerRT.gameObject.SetActive(false);
            return;
        }
        markerRT.gameObject.SetActive(true);

        Vector2 uiPos = ViewportToMiniMapUI(vp);
        markerRT.anchoredPosition = uiPos;

        // 회전 반영!
        markerRT.rotation = Quaternion.Euler(0, 0, -target.eulerAngles.y);
    }


    Vector2 ViewportToMiniMapUI(Vector3 vp)
    {
        // (0,0)~(1,1) → (-½W, -½H)~(½W, ½H)
        float x = (vp.x - 0.5f) * miniMapRect.sizeDelta.x;
        float y = (vp.y - 0.5f) * miniMapRect.sizeDelta.y;
        return new Vector2(x, y);
    }
}
