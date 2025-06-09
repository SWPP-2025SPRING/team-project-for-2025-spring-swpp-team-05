using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelObstacle : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody playerRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            if (playerRigidbody != null)
            {
                // 플레이어에게 힘을 가하여 밀어내기
                Vector3 pushDirection = collision.transform.position - transform.position;
                pushDirection.y = 0; // Y축 방향은 무시
                playerRigidbody.AddForce(pushDirection.normalized * 2000f, ForceMode.Impulse);
                Destroy(gameObject); // 충돌 후 장애물 제거
            }
        }
    }
}
