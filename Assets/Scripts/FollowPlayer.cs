using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject player;
    private Vector3 rearViewOffset = new Vector3(0f, 5f, -15f);          // 2번 - 뒤에서 위
    private Vector3 leftViewOffset = new Vector3(-4f, 5f, -13f);          // 1번 - 왼쪽, 위, 앞
    private Vector3 rightViewOffset = new Vector3(4f, 5f, -13f);          // 3번 - 오른쪽, 위, 앞

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
        transform.position = player.transform.TransformPoint(currentOffset);
        transform.LookAt(player.transform.position + Vector3.up * 2f); 
    }
}