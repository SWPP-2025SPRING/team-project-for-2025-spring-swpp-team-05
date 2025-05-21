using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReportController : MonoBehaviour
{
    public float speed = 0.5f;
    private GameObject player;
    private Rigidbody reportRigidbody;
    private Animator reportAnimator;

    public float launchForce = 70f;
    public float horizontalForce = 3f;
    public float homingSpeed = 5f;
    public float detectionRadius = 5f;
    public float attatchDistance = 0.3f;



    private bool isLaunched = false;
    private bool isAttatched = false;
    private Vector3 attachOffset;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        reportRigidbody = GetComponent<Rigidbody>();
        reportAnimator = GetComponentInChildren<Animator>();
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
                AttackStart();
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
    }


    void AttackStart()
    {
        Debug.Log("Attached");
        isAttatched = true;
        reportRigidbody.velocity = Vector3.zero; // 속도 초기화
        attachOffset = transform.position - player.transform.position; // 플레이어와의 상대 위치 저장
        transform.SetParent(player.transform); // 플레이어의 자식으로 설정
        Destroy(reportRigidbody);
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
}