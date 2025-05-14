using System.Collections;
using UnityEngine;

public class ProfessorController : MonoBehaviour
{
    private bool facingForward = true;

    void Start()
    {
        StartCoroutine(PatrolRoutine());
    }

    IEnumerator PatrolRoutine()
    {
        while (true)
        {
            // 정지 시간: 2초 또는 1초
            float waitTime = facingForward ? 1.5f : 1f;
            yield return new WaitForSeconds(waitTime);

            // 현재 방향 기준으로 목표 회전 계산
            Quaternion startRot = transform.rotation;
            Quaternion endRot = startRot * Quaternion.Euler(0f, 180f, 0f);

            // 회전 시간과 속도 설정
            float rotateDuration = 0.2f;
            float elapsed = 0f;

            while (elapsed < rotateDuration)
            {
                transform.rotation = Quaternion.Slerp(startRot, endRot, elapsed / rotateDuration);
                elapsed += Time.deltaTime;
                yield return null;
            }

            // 정확히 도달하지 못할 경우를 위해 마지막 값 보정
            transform.rotation = endRot;

            // 다음 단계에서 반대 방향 대기 시간 사용하도록 플래그 토글
            facingForward = !facingForward;
        }
    }
}
