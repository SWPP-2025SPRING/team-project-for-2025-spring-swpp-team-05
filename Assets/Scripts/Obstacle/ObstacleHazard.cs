using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ObstacleHazard : MonoBehaviour
{
    public float knockbackForce = 30f;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();

            if (playerRb != null)
            {
                // 충돌 지점 기준 방향 계산
                Vector3 direction = collision.gameObject.transform.position - transform.position;
                direction.y = 0f; // 수평 방향만 적용 (위로 튀지 않게)

                playerRb.AddForce(direction.normalized * knockbackForce, ForceMode.Impulse);
            }
        }
    }
}
