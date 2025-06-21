using System.Collections;
using UnityEngine;

public class ProfessorController : MonoBehaviour, IMonsterController
{
    private bool facingForward = true;
    public float rotateDuration = 0.2f;
    public float facingForwardTime = 1.5f;
    public float facingBackwardTime = 1f;
    public float fieldOfView = 60f; // 시야각
    public float stunTime = 2f; // 스턴 시간

    public int maxHP = 100;
    public int currentHP;
    private Animator animator;

    private GameObject player;

    private Vector3 lastPlayerPosition;

    public void SetLevel(int level)
    {
        // Example growth rates, adjust as needed
        float facingForwardTimeGrowth = 0.05f;
        float facingBackwardTimeGrowth = 0.03f;
        float stunTimeGrowth = 0.1f;

        facingForwardTime += facingForwardTime * facingForwardTimeGrowth * level;
        facingBackwardTime += facingBackwardTime * facingBackwardTimeGrowth * level;
        stunTime += stunTime * stunTimeGrowth * level;
    }

    void Start()
    {
        currentHP = maxHP;
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player GameObject not found! Make sure the player has the tag 'Player'.");
        }
        animator = GetComponent<Animator>();
        StartCoroutine(PatrolRoutine());
    }

    void Update()
    {
        if (player == null)
        {
            return;
        }
        if (!facingForward && IsPlayerInView() && Vector3.Distance(player.transform.position, lastPlayerPosition) > 0.1f)
        {
            Debug.Log("Player Movement Detected - Attacking!");
            Attack(stunTime);
        }
        lastPlayerPosition = player.transform.position;
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
            if (elapsed >= rotateDuration)
            {
                facingForward = !facingForward;
            }
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

    void Attack(float stunTime)
    {
        PlayerStatus.instance.StunPlayer(stunTime);
    }

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

    public bool IsPlayerInView()
    {
        if (player == null) return false;
        Debug.Log("Checking if player is in view...");

        Vector3 directionToPlayer = player.transform.position - transform.position;
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

        // 플레이어가 시야각 내에 있는지 확인
        return angleToPlayer < fieldOfView / 2f;
    }
    // ...existing code...

    public void EndMonster()
    {
        // Implement any cleanup or end logic for the monster here
        Debug.Log("ProfessorController EndMonster called.");
        Destroy(gameObject); // Example: destroy the monster GameObject
    }
}