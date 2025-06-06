using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderWebSlowZone : MonoBehaviour
{
    [Tooltip("공격력이 0일 때의 최대 슬로우율 (0~1 사이)")]
    [Range(0f, 1f)]
    public float maxSlowRate = 0.5f;

    [Tooltip("슬로우가 지속되는 시간 (초)")]
    public float slowDuration = 2.0f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            float attackPowerRatio = PlayerStatus.instance.GetAttackPowerRatio();  // 0.0 ~ 1.0
            float appliedSlowRate = maxSlowRate * (1 - attackPowerRatio);

            // 슬로우 적용
            PlayerStatus.instance.SlowPlayer(appliedSlowRate);

            // 일정 시간 후 슬로우 복구
            StartCoroutine(RecoverAfterDelay(appliedSlowRate));
        }
    }

    private System.Collections.IEnumerator RecoverAfterDelay(float slowRate)
    {
        yield return new WaitForSeconds(slowDuration);
        PlayerStatus.instance.ReviveSlow(slowRate);
    }
}
