using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public static PlayerStatus instance { get; private set; }

    public float moveSpeed { get; private set; }
    public float attackPower { get; private set; }
    public float attackRange { get; private set; }

    public float defaultMoveSpeed = 10.0f;
    public float defaultAttackPower = 10;
    public float defaultAttackRange = 3.0f;

    private float slowRateAgg = 0f;
    public bool isSlow { get; private set; } = false;
    public bool isStun { get; private set; } = false;

    public int level { get; private set; } = 1;

    //public int exp { get; private set; } = 0;
    //public int nextExp { get; private set; } = 100;

    // 🔐 고정 상수 (외부 수정 방지)
    // TODO: 공격 스탯 필요없다는 결론 나오면 다 삭제
    public static readonly float maxAttackPower = 100f;
    public static readonly float maxAttackRange = 10f;
    public static readonly float minMoveSpeed = 3f;
    public static readonly float maxMoveSpeed = 30f;
    public static readonly float speedStep = 2f;
    public static readonly float attackStep = 1f;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        moveSpeed = defaultMoveSpeed;
        attackPower = defaultAttackPower;
        attackRange = defaultAttackRange;
        DontDestroyOnLoad(gameObject);
    }

    public void SlowPlayer(float slowRate)
    {
        if (!isSlow)
        {
            isSlow = true;
        }
        slowRateAgg += slowRate;
        moveSpeed = defaultMoveSpeed * (1 - slowRateAgg);
        if (moveSpeed < 0)
        {
            moveSpeed = 0.01f;
        }
    }

    public void ReviveSlow(float slowRate)
    {
        slowRateAgg -= slowRate;
        if (slowRateAgg < 0)
        {
            slowRateAgg = 0;
        }
        moveSpeed = defaultMoveSpeed * (1 - slowRateAgg);
        if (moveSpeed < 0)
        {
            moveSpeed = 0.01f;
        }
        if (slowRateAgg < 0.01f)
        {
            isSlow = false;
        }
    }

    public void StunPlayer(float stunTime)
    {
        isStun = true;
        StartCoroutine(StunPlayerCoroutine(stunTime));
    }

    IEnumerator StunPlayerCoroutine(float stunTime)
    {
        float tempSpeed = moveSpeed;
        moveSpeed = 0;
        yield return new WaitForSeconds(stunTime);
        moveSpeed = tempSpeed;
        isStun = false;
    }

    public void IncreaseAttackPower()
    {
        attackPower = Mathf.Min(attackPower + attackStep, maxAttackPower);
    }

    public void IncreaseAttackRange()
    {
        attackRange = Mathf.Min(attackRange + attackStep, maxAttackRange);
    }

    public float GetAttackPowerRatio()
    {
        return Mathf.Clamp01(attackPower / maxAttackPower);
    }

    public float GetAttackRangeRatio()
    {
        return Mathf.Clamp01(attackRange / maxAttackRange);
    }

    public void IncreaseSpeed()
    {
        moveSpeed = Mathf.Min(moveSpeed + speedStep, maxMoveSpeed);
    }

    public void DecreaseSpeed()
    {
        moveSpeed = Mathf.Max(moveSpeed - speedStep, minMoveSpeed);
    }
}
