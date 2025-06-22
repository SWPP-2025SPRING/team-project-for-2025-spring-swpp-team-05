using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMove : MonoBehaviour
{
    public Vector2 moveDirection = new Vector2(1, 0); // 입력 방향
    public float leftRange = 0f;    // 왕복 시작점 (0이면 현재 위치)
    public float rightRange = 5f;   // 이동 거리 (이 방향으로 얼마나 갈지)
    public float cycleDuration = 2f; // 왕복 주기 (왕복 전체 걸리는 시간)
    private Vector3 startPosition;  // 초기 위치 저장
    private float timer = 0f;

    void Start()
    {
        startPosition = transform.position;
    }

    void LateUpdate()
    {
        timer += Time.deltaTime;

        // 왕복을 위한 t 값: 0 → 1 → 0 반복
        float t = Mathf.PingPong(timer / cycleDuration, 1f);

        // ease-in-out 적용
        float easedT = t * t * (3f - 2f * t);

        // 최종 이동 위치 계산
        Vector3 offset = moveDirection * Mathf.Lerp(leftRange, rightRange, easedT);
        transform.position = startPosition + offset;
    }


    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(transform); // 플레이어를 플랫폼에 붙임
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null); // 플레이어를 플랫폼에서 분리
        }
    }
}