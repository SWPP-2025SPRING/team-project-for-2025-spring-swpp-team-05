using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelObstacle : MonoBehaviour
{
    public AudioClip barrelCollisionSound; // 장애물과 충돌 시 재생할 소리
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
                if (!PlayerStatus.instance.isBoost)
                {
                    // 플레이어에게 힘을 가하여 밀어내기
                    Vector3 pushDirection = collision.transform.position - transform.position;
                    if (Mathf.Abs(pushDirection.x) > Mathf.Abs(pushDirection.z))
                    {
                        pushDirection.z = 0;
                    }
                    else
                    {
                        pushDirection.x = 0;
                    }
                    playerRigidbody.AddForce(pushDirection.normalized * 10, ForceMode.Impulse);
                }
                SoundEffectManager.Instance.PlayOneShotOnce(barrelCollisionSound); // 장애물과 충돌 시 소리 재생
                Destroy(gameObject); // 충돌 후 장애물 제거
            }
        }
    }
}
