using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReportController : MonoBehaviour, IMonsterController
{
    [Header("Report Settings")]
    private GameObject player;
    private Rigidbody reportRigidbody;
    private Animator reportAnimator;

    [Header("Report Status Settings(Base)")]
    public float speed = 0.5f;
    public float launchForce = 70f;
    public float horizontalForce = 3f;
    public float homingSpeed = 5f;
    public float detectionRadius = 5f;

    [Header("Report Attack Settings")]
    public float attatchDistance = 0.3f;
    public float stunRate = 10f;
    public int codeLength = 6;
    private SolveCode solveCode;

    [Header("Sound Effects")]
    public AudioClip launchSound;
    public AudioClip attachSound;


    [Header("Flags & Temps")]
    private bool isLaunched = false;
    private bool isAttatched = false;
    private Vector3 attachOffset;

    public void SetLevel(int level)
    {
        // --- Growth Rates ---
        float speedGrowth = 0.05f;
        float homingSpeedGrowth = 2f;          // applies to log growth
        float stunRateGrowth = 0.05f;
        int codeLengthGrowth = 1;

        // --- Apply Growth ---
        speed += speed * speedGrowth * level;
        homingSpeed += homingSpeed * homingSpeedGrowth * level;
        stunRate += stunRateGrowth * level;
        codeLength += codeLengthGrowth * level;
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        reportRigidbody = GetComponent<Rigidbody>();
        reportAnimator = GetComponentInChildren<Animator>();
        solveCode = new SolveCode(gameObject, codeLength, stunRate);
    }

    // Update is called once per frame
    void Update()
    {
        if (isAttatched)
        {
            Vector3 targetPosition = player.transform.position + attachOffset;
            reportAnimator.SetBool("Attack_b", true);
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * homingSpeed);
            return;
        }

        float distance = Vector3.Distance(player.transform.position, transform.position);
        if (distance < detectionRadius && !isLaunched)
        {
            Launch();
        }

        if (isLaunched && !isAttatched)
        {
            Vector3 direction = (player.transform.position - transform.position).normalized;
            reportRigidbody.velocity = Vector3.Lerp(reportRigidbody.velocity, direction * homingSpeed, Time.deltaTime * 1.5f);

            if (distance < attatchDistance)
            {
                OnAttack();
            }
        }
        if (!isLaunched && !isAttatched)
        {
            ChasePlayer();
        }
    }


    void Launch()
    {
        isLaunched = true;
        reportAnimator.SetBool("Prepare_b", true);
        Vector3 direction = (player.transform.position - transform.position).normalized;
        //direction.y = 0; // y축 방향 제거
        Vector3 force = direction * launchForce + Vector3.up * launchForce;
        reportRigidbody.velocity = Vector3.zero; // 초기 속도 초기화
        reportRigidbody.AddForce(force, ForceMode.VelocityChange);
        SoundEffectManager.Instance.PlayOneShotOnce(launchSound); // 발사 사운드 재생
    }


    public void OnAttack()
    {
        Debug.Log("Attached");
        isAttatched = true;
        reportRigidbody.velocity = Vector3.zero; // 속도 초기화
        attachOffset = transform.position - player.transform.position; // 플레이어와의 상대 위치 저장
        Destroy(reportRigidbody);
        player.GetComponent<PlayerControl>().StunPlayer(solveCode);
        SoundEffectManager.Instance.PlayOneShotOnce(attachSound); // 부착 사운드 재생
    }

    void ChasePlayer()
    {
        Vector3 currentPosition = transform.position;
        Vector3 playerPosition = player.transform.position;
        Vector3 chaseDirection = playerPosition - currentPosition;
        chaseDirection.y = 0; // y축 방향 제거

        reportRigidbody.velocity = (chaseDirection).normalized * speed;
        reportAnimator.SetFloat("Speed_f", 0.6f);

        Quaternion targetRotation = Quaternion.LookRotation(chaseDirection.normalized, Vector3.up);
        Quaternion correction = Quaternion.Euler(0, 90, 0); // 임시 조치

        transform.rotation = targetRotation * correction;
    }

    public void EndMonster()
    {
        Destroy(gameObject);
    }
}