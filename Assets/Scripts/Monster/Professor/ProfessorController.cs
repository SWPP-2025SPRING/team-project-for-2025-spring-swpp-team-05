using System.Collections;
using UnityEngine;

public class ProfessorController : MonoBehaviour, IMonsterController
{
    public bool facingBackward = false;
    public float rotateDuration = 0.2f;
    public float facingForwardTime = 1.5f;
    public float facingBackwardTime = 1f;
    public float fieldOfView = 60f; // 시야각
    public float stunTime = 2f; // 스턴 시간
    public float speedThreshold = 5f;
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
        if (facingBackward && player.transform.position.z < transform.position.z)
        {
            CheckPlayerMovement();
            lastPlayerPosition = player.transform.position;
        }
    }

    IEnumerator PatrolRoutine()
    {
        while (true)
        {
            // 정지 시간: 2초 또는 1초
            float waitTime = !facingBackward ? facingForwardTime : facingBackwardTime;
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
                facingBackward = !facingBackward;
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

    void Stun(float stunTime)
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

    private void CheckPlayerMovement()
    {
        float playerSpeed = GetPlayerSpeed();
        if (playerSpeed > speedThreshold)
        {
            Debug.Log("Player is moving too fast! Stunned!");
            Stun(stunTime); // 플레이어를 스턴
        }
    }

    private float GetPlayerSpeed()
    {
        // 플레이어의 속도를 계산
        Vector3 playerVelocity = (player.transform.position - lastPlayerPosition) / Time.deltaTime;
        return playerVelocity.magnitude; // 속도의 크기 반환
    }

    public void EndMonster()
    {
        // Implement any cleanup or end logic for the monster here
        Destroy(gameObject); // Example: destroy the monster GameObject
    }
}