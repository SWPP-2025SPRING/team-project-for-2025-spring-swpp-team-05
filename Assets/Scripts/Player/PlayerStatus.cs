using Codice.Client.Commands;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerStatus : MonoBehaviour
{
    public static PlayerStatus instance { get; private set; }

    public float moveSpeed { get; private set; }

    public float maxSpeed { get; private set; }
    public float minSpeed { get; private set; }
    public float acceleration { get; private set; }
    public float deceleration { get; private set; }

    public int generalExp { get; private set; } = 0;
    public int peExp { get; private set; } = 0;
    public int majorExp { get; private set; } = 0;
    public int thesisExp { get; private set; } = 0;
    public int credit { get; private set; } = 0;
    public float spa { get; private set; } = 0f;

    public float defaultMoveSpeed = 10.0f;
    public float defaultMinSpeed = 3.0f;
    public float defaultAcceleration = 5.0f;
    public float defaultDeceleration = 5.0f;

    private float slowRateAgg = 0f;
    public bool isSlow { get; private set; } = false;
    public bool isStun { get; private set; } = false;
    public bool isReverseControl { get; private set; } = false;
    public bool isStop { get; private set; } = false;

    public int level { get; private set; } = 1;
    public int maxLevel { get; private set; } = 50;


    public float attackGrowthRate = 1.1f; // 공격력 성장률
    public float attackRangeGrowthRate = 1.1f; // 공격 범위 성장률
    public float speedGrowthRate = 0.5f; // 속도 성장률
    public float accelGrowthRate = 0.2f; // 가속도 성장률
    public float decelGrowthRate = 0.2f; // 감속도 성장률

    public static readonly float speedStep = 1.5f;
    public static readonly float attackStep = 1f;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        moveSpeed = defaultMinSpeed;
        maxSpeed = defaultMoveSpeed;
        acceleration = defaultAcceleration;
        deceleration = defaultDeceleration;
        DontDestroyOnLoad(gameObject);
    }

    public void ResetPlayerStatus()
    {
        moveSpeed = defaultMoveSpeed;
        maxSpeed = defaultMoveSpeed;
        minSpeed = defaultMinSpeed;
        acceleration = defaultAcceleration;
        deceleration = defaultDeceleration;
    }

    public void LevelUp(MonsterType type, int levelIncrement = 1, int per = 3)
    {
        List<float> statusBefore = GetStatusList();
        level += levelIncrement;
        if (level > maxLevel)
        {
            level = maxLevel;
        }
        maxSpeed = defaultMoveSpeed + (level - 1) * speedGrowthRate;
        acceleration = defaultAcceleration + (level - 1) * accelGrowthRate;
        deceleration = defaultDeceleration + (level - 1) * decelGrowthRate;

        switch (type)
        {
            case MonsterType.Report:
                generalExp += per;
                break;
            case MonsterType.Professor:
                thesisExp += per;
                break;
            case MonsterType.Python:
                majorExp += per;
                break;
            case MonsterType.PE:
                peExp += per;
                break;
        }

        List<float> statusAfter = GetStatusList();
        GameManager.Instance.uiManager.UpdateLevel(level);
        StatusUIManager.Instance.ShowStatusUpdate(statusBefore, statusAfter);
    }

    public void SlowPlayer(float slowRate)
    {
        if (!isSlow)
        {
            isSlow = true;
            DebufManager.Instance.UpdateDebufText(DebufType.Slow);
        }
        slowRateAgg += slowRate;
        maxSpeed = GetMaxSpeed() * (1 - slowRateAgg);
        if (maxSpeed < 0)
        {
            maxSpeed = 0.01f;
        }
    }

    public void ReviveSlow(float slowRate)
    {
        slowRateAgg -= slowRate;
        if (slowRateAgg < 0)
        {
            slowRateAgg = 0;
        }
        maxSpeed = GetMaxSpeed() * (1 - slowRateAgg);
        if (maxSpeed < 0)
        {
            maxSpeed = 0.01f;
        }
        if (slowRateAgg < 0.01f)
        {
            isSlow = false;
            DebufManager.Instance.UpdateDebufText(DebufType.None);
        }
    }

    public void StunPlayer(float stunTime)
    {
        if (isStun) return;
        StartCoroutine(StunPlayerCoroutine(stunTime));
    }


    IEnumerator StunPlayerCoroutine(float stunTime)
    {
        isStun = true;
        isStop = true;

        float tempSpeed = moveSpeed;
        moveSpeed = 0;

        // 디버프 UI 반영
        DebufManager.Instance.UpdateDebufText(DebufType.Stun);

        // 해당 시간 동안 완전 멈춤
        yield return new WaitForSeconds(stunTime);

        // 복구
        isStun = false;
        isStop = false;
        moveSpeed = tempSpeed;

        DebufManager.Instance.UpdateDebufText(DebufType.None);
        Debug.Log("Player recovered from stun.");
    }


    public void SetReverseControl(bool isReverse)
    {
        isReverseControl = isReverse;
        if (isReverse)
        {
            DebufManager.Instance.UpdateDebufText(DebufType.ControlInversion);
        }
        else
        {
            DebufManager.Instance.UpdateDebufText(DebufType.None);
        }
    }

    public void Accelerate(float factor, float dt)
    {
        if (isStop || isStun) return;

        float targetSpeed = maxSpeed * factor;
        if (moveSpeed < targetSpeed)
        {
            moveSpeed = Mathf.MoveTowards(moveSpeed, targetSpeed, acceleration * dt);
        }
        else if (moveSpeed > targetSpeed)
        {
            moveSpeed = Mathf.MoveTowards(moveSpeed, targetSpeed, deceleration * dt);
        }
        moveSpeed = Mathf.Clamp(moveSpeed, minSpeed, maxSpeed);
    }

    public void DeAccelerate(float factor, float dt)
    {
        if (isStop || isStun) return;

        float targetSpeed = minSpeed * factor;
        if (moveSpeed > targetSpeed)
        {
            moveSpeed = Mathf.MoveTowards(moveSpeed, targetSpeed, deceleration * dt);
        }
        else if (moveSpeed < targetSpeed)
        {
            moveSpeed = Mathf.MoveTowards(moveSpeed, targetSpeed, acceleration * dt);
        }
        moveSpeed = Mathf.Clamp(moveSpeed, minSpeed, maxSpeed);
    }

    public void IncreaseSpeed()
    {
        moveSpeed = Mathf.Min(moveSpeed + speedStep, maxSpeed);
    }

    public void DecreaseSpeed()
    {
        moveSpeed = Mathf.Max(moveSpeed - speedStep, minSpeed);
    }
    private float GetMaxSpeed()
    {
        return defaultMoveSpeed + (level - 1) * speedGrowthRate;
    }

    public void StopPlayer()
    {
        isStop = true;
        moveSpeed = 0;
    }

    public void ResumePlayer()
    {
        isStop = false;
    }

    public void SetSpeedZero()
    {
        moveSpeed = 0;
    }

    private List<float> GetStatusList()
    {
        List<float> statusList = new List<float>
        {
            level,
            maxSpeed,
            acceleration,
            deceleration,
            generalExp,
            peExp,
            majorExp,
            thesisExp,
            credit,
            spa
        };
        return statusList;
    }
}
