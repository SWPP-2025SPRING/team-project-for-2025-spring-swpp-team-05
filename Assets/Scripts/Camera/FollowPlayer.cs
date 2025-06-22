using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject player;
    public Vector3 rearViewOffset = new Vector3(0f, 1f, -13f);          // 2번 - 뒤에서 위
    private Vector3 leftViewOffset = new Vector3(-4f, 5f, -13f);          // 1번 - 왼쪽, 위, 앞
    private Vector3 rightViewOffset = new Vector3(4f, 5f, -13f);          // 3번 - 오른쪽, 위, 앞

    public float followSpeed = 5f; // 카메라가 따라가는 속도
    public float rotationSpeed = 5f; // 카메라가 회전하는 속도

    private Vector3 velocity = Vector3.zero; // SmoothDamp를 위한 속도 변수

    private Vector3 currentOffset;
    private Quaternion fixedRotation;

    // for Python Monster Error
    private bool isFlipped = false;
    private float externalTiltAngle = 0f;
    private Camera cam;

    [Header("End Scene Settings")]
    private bool isEndScene = false;
    public float duration = 5.0f; // 전체 연출 시간
    public float lookSpeed = 2.0f; // 회전 속도

    // Start is called before the first frame update
    void Start()
    {
        currentOffset = rearViewOffset; // 기본은 2번 시점
        fixedRotation = transform.rotation;

        cam = Camera.main;
        if (cam == null)
        {
            return;
        }
        float[] distances = new float[32];
        distances[LayerMask.NameToLayer("Props")] = 100f; // Props 레이어는 100m 거리
        distances[LayerMask.NameToLayer("Default")] = 1000f; // Default 레이어는 100m 거리
        distances[LayerMask.NameToLayer("Building")] = 1000f; // Player 레
        cam.layerCullDistances = distances;
    }

    // Update is called once per frame
    void Update()
    {
        if (isEndScene) return;
        // 1: left
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentOffset = leftViewOffset;
        }

        // 2: behind
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentOffset = rearViewOffset;
        }

        // 3: right
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentOffset = rightViewOffset;
        }
    }

    void LateUpdate()
    {
        if (player == null || isEndScene) return;

        Vector3 targetPosition = player.transform.TransformPoint(currentOffset);
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, 1f / followSpeed);

        Quaternion lookRotation = Quaternion.LookRotation(player.transform.position - transform.position);
        lookRotation *= Quaternion.Euler(0, 0, externalTiltAngle);

        if (isFlipped)
        {
            lookRotation *= Quaternion.Euler(0, 0, 180);
        }

        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);

    }

    public void SetFlipped(bool flip)
    {
        isFlipped = flip;
    }

    public void SetTiltAngle(float angle)
    {
        externalTiltAngle = angle;
    }

    public void SetEndScene(GameObject player2)
    {
        if (isEndScene) return; // 이미 엔드 씬이 진행 중이면 무시

        isEndScene = true;
        StartCoroutine(EndScene(player2));
    }

    public IEnumerator EndScene(GameObject player2)
    {
        Vector3 playerPos = player2.transform.position;

        // 시작 위치: player2보다 위쪽 (푸른 하늘)
        Vector3 startPos = playerPos + new Vector3(0, 20f, 0);
        Quaternion startRot = Quaternion.LookRotation(Vector3.up); // 하늘 방향

        // 도착 위치: player2 앞쪽에서 약간 떨어진 곳
        Vector3 endPos = playerPos + player2.transform.forward * 10f + Vector3.up * 3f;
        Quaternion endRot = Quaternion.LookRotation(playerPos + Vector3.up * 3f - endPos); // player2를 바라보는 회전

        float elapsed = 0f;

        // 초기 위치/회전 세팅
        transform.position = startPos;
        transform.rotation = startRot;

        while (elapsed < duration)
        {
            float t = elapsed / duration;

            // 카메라 이동 & 회전 보간
            transform.position = Vector3.Lerp(startPos, endPos, t);
            transform.rotation = Quaternion.Slerp(startRot, endRot, t);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // 마지막 위치 정확히 정렬
        transform.position = endPos;
        transform.rotation = endRot;
    }
}