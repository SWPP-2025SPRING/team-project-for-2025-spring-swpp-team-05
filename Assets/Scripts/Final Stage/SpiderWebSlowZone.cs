using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderWebSlowZone : MonoBehaviour
{
    public float knockbackDistance = 12f;
    public float knockbackDuration = 0.2f;

    public int maxHitCount = 3;

    private int currentHitCount = 0;
    private bool isCooldown = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player") && !isCooldown)
        {
            currentHitCount++;
            StartCoroutine(Knockback(collision.collider.gameObject));

            if (currentHitCount >= maxHitCount)
            {
                Destroy(gameObject); // 거미줄 제거
            }
        }
    }

    private System.Collections.IEnumerator Knockback(GameObject player)
    {
        isCooldown = true;

        Vector3 dir = (player.transform.position - transform.position).normalized;
        dir.z = -2.0f;
        dir.y = 0.02f;
        dir.Normalize();

        Vector3 targetPos = player.transform.position + dir * knockbackDistance;

        float elapsed = 0f;
        float duration = knockbackDuration;

        Vector3 start = player.transform.position;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            // Ease-out curve: 느리게 끝나도록 조절
            float easedT = 1f - Mathf.Pow(1f - t, 2f); // (1 - (1-t)^2)
            player.transform.position = Vector3.Lerp(start, targetPos, easedT);

            elapsed += Time.deltaTime;
            yield return null;
        }

        player.transform.position = targetPos;

        yield return new WaitForSeconds(0.2f); // 짧은 쿨타임
        isCooldown = false;
    }

}
