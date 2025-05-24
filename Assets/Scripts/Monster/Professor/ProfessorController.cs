using System.Collections;
using UnityEngine;

public class ProfessorController : MonoBehaviour
{
    private bool facingForward = true;
    public float rotateDuration = 0.2f;
    public float facingForwardTime = 1.5f;
    public float facingBackwardTime = 1f;
    public int maxHP = 100;
    public int currentHP;
    private Animator animator;


    void Start()
    {
        currentHP = maxHP;
        animator = GetComponent<Animator>();
        StartCoroutine(PatrolRoutine());
    }

    IEnumerator PatrolRoutine()
    {
        while (true)
        {
            // 정지 시간: 2초 또는 1초
            float waitTime = facingForward ? facingForwardTime : facingBackwardTime;
            yield return new WaitForSeconds(waitTime);

            // 현재 방향 기준으로 목표 회전 계산
            Quaternion startRot = transform.rotation;
            Quaternion endRot = startRot * Quaternion.Euler(0f, 180f, 0f);

            // 회전 시간과 속도 설정
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

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Attacked(1); // Decrease HP by 1 (you can change the damage value)
        }
    }

    // ...existing code...

    public void Attacked(int damage)
    {
        currentHP -= damage;
        if (animator != null)
        {
            animator.SetBool("Attacked", true);
            StartCoroutine(ResetAttackedFlag());
        }
        if (currentHP <= 0)
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator ResetAttackedFlag()
    {
        yield return new WaitForSeconds(0.2f); // 0.2초 후에 false로 변경 (원하는 시간으로 조정)
        if (animator != null)
        {
            animator.SetBool("Attacked", false);
        }
    }
    // ...existing code...
}