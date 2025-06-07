using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject player;
    private Vector3 rearViewOffset = new Vector3(0f, 500f, -1300f);          // 2번 - 뒤에서 위
    private Vector3 leftViewOffset = new Vector3(-4f, 5f, -13f);          // 1번 - 왼쪽, 위, 앞
    private Vector3 rightViewOffset = new Vector3(4f, 5f, -13f);          // 3번 - 오른쪽, 위, 앞

    public float followSpeed = 5f; // 카메라가 따라가는 속도
    public float rotationSpeed = 5f; // 카메라가 회전하는 속도

    private Vector3 velocity = Vector3.zero; // SmoothDamp를 위한 속도 변수

    private Vector3 currentOffset;
    private Quaternion fixedRotation;

    // Start is called before the first frame update
    void Start()
    {
        currentOffset = rearViewOffset; // 기본은 2번 시점
        fixedRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
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
        if (player == null) return;

        Vector3 targetPosition = player.transform.TransformPoint(currentOffset);
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, 1f / followSpeed);

        Quaternion lookRotation = Quaternion.LookRotation(player.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);

    }
}