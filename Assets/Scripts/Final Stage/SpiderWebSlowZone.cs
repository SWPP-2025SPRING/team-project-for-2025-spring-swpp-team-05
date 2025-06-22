using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderWebSlowZone : MonoBehaviour
{
    public AudioClip webCollisionSound; // 거미줄 장애물과 충돌 시 재생할 소리
    public float knockbackForce = 32f; // 밀려나는 힘
    public float upwardForce = 9f;     // 살짝 위로 튕기는 느낌
    public float cooldownTime = 0.5f;

    public int maxHitCount = 3;

    private int currentHitCount = 0;
    private bool isCooldown = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player") && !isCooldown)
        {
            Rigidbody playerRb = collision.collider.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                currentHitCount++;
                SoundEffectManager.Instance.PlayOneShotOnce(webCollisionSound); // 거미줄 장애물과 충돌 시 소리 재생

                // 1. 방향은 수평(xz 평면) 기준으로 계산
                Vector3 horizontalDir = collision.collider.transform.position - transform.position;
                horizontalDir.y = 0f; // 위 방향 제거
                horizontalDir.Normalize();

                // 2. 수평 + 위로 살짝 힘 분리
                Vector3 knockback = horizontalDir * knockbackForce + Vector3.up * upwardForce;

                playerRb.AddForce(knockback, ForceMode.Impulse);

                StartCoroutine(CooldownCoroutine());
                StartCoroutine(ShakeWeb());

                if (currentHitCount >= maxHitCount)
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    private IEnumerator CooldownCoroutine()
    {
        isCooldown = true;
        yield return new WaitForSeconds(cooldownTime);
        isCooldown = false;
    }

    private IEnumerator ShakeWeb()
    {
        float duration = 0.6f;
        float frequency = 60f;
        float scaleXIntensity = 0.04f;
        float scaleYIntensity = 0.03f;
        float positionXIntensity = 0.03f;

        Vector3 originalScale = transform.localScale;
        Vector3 originalPos = transform.localPosition;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            float pulse = Mathf.Sin(elapsed * frequency);

            // Scale: 좌우 / 상하로 튀는 느낌
            float scaleX = originalScale.x + pulse * scaleXIntensity;
            float scaleY = originalScale.y + pulse * scaleYIntensity;

            // Position: 좌우 떨림
            float posX = originalPos.x + pulse * positionXIntensity;

            transform.localScale = new Vector3(scaleX, scaleY, originalScale.z);
            transform.localPosition = new Vector3(posX, originalPos.y, originalPos.z);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // 원래대로 복구
        transform.localScale = originalScale;
        transform.localPosition = originalPos;
    }

}
