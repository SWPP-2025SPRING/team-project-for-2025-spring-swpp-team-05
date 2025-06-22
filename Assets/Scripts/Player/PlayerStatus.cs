using Codice.Client.Commands;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerStatus : MonoBehaviour
{
    public AudioClip levelUpSound; // Sound played when the player levels up
    public AudioClip boostSound; // Sound played when the player uses a booster
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

    public bool isBoost { get; private set; } = false;
    public bool isFinal { get; private set; } = false; // 최종 레벨 도달 여부


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
        maxSpeed = defaultMoveSpeed * 1.5f;
        minSpeed = defaultMinSpeed;
        acceleration = defaultAcceleration;
        deceleration = defaultDeceleration;
        isFinal = true;
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

        SoundEffectManager.Instance.PlayOneShotOnce(levelUpSound);
        List<float> statusAfter = GetStatusList();
        GameManager.Instance.uiManager.UpdateLevel(level);
        StatusUIManager.Instance.ShowStatusUpdate(statusBefore, statusAfter);
    }

    public void SlowPlayer(float slowRate)
    {
        if (!isSlow)
        {
            isSlow = true;
            DebufManager.Instance.UpdateDebufText(DebufType.Stun);
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

    public void ResetSlow()
    {
        slowRateAgg = 0f;
        maxSpeed = GetMaxSpeed();
        isSlow = false;
        DebufManager.Instance.UpdateDebufText(DebufType.None);
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
        if (isStop) return; // 가속 중지 상태면 가속하지 않음
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
        GameManager.Instance.uiManager.UpdateSpeed(moveSpeed);
    }

    public void DeAccelerate(float factor, float dt)
    {
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
        GameManager.Instance.uiManager.UpdateSpeed(moveSpeed);
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

    public void Booster()
    {
        if (!isFinal)
        {
            Debug.LogWarning("최종 레벨에 도달하여 부스터를 사용할 수 없습니다.");
            return;
        }
        if (level < 5)
        {
            Debug.LogWarning("레벨이 너무 낮아 부스터를 사용할 수 없습니다.");
            return;
        }
        if (isBoost)
        {
            Debug.LogWarning("이미 부스터를 사용 중입니다.");
            return;
        }
        StartCoroutine(UseBooster());
    }

    private IEnumerator UseBooster()
    {
        isBoost = true;
        float originalSpeed = moveSpeed;
        float originalMaxSpeed = maxSpeed;
        moveSpeed *= 2;
        maxSpeed *= 2f;
        level -= 5; // 레벨을 5 감소
        GameManager.Instance.uiManager.UpdateSpeed(moveSpeed);
        SoundEffectManager.Instance.PlayOneShotOnce(boostSound);
        GameManager.Instance.uiManager.UpdateLevel(level);
        yield return new WaitForSeconds(4f); // 5초 동안 부스트 효과
        moveSpeed = originalSpeed;
        maxSpeed = originalMaxSpeed;
        isBoost = false;
        GameManager.Instance.uiManager.UpdateSpeed(moveSpeed);
    }


}
