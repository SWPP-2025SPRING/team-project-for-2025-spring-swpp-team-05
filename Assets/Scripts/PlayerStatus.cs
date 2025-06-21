using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public static PlayerStatus instance { get; private set; }

    public float moveSpeed { get; private set; }

    public float defaultMoveSpeed = 10.0f;

    private float slowRateAgg = 0f;
    public bool isSlow { get; private set; } = false;
    public bool isStun { get; private set; } = false;
    public bool isReverseControl { get; private set; } = false;

    public int level { get; private set; } = 1;
    public int maxLevel { get; private set; } = 50;

    public float speedGrowthRate = 1.2f; // 속도 성장률



    //public int exp { get; private set; } = 0;
    //public int nextExp { get; private set; } = 100;

    // 🔐 고정 상수 (외부 수정 방지)
    public static readonly float minMoveSpeed = 3f;
    // 1. 현재 레벨에 따른 max Speed
    public static readonly float maxMoveSpeed = 30f;
    // TODO: 레벨 상관없이 최대 Speed도 알아야됨 (uiManager과 일치시키기)
    public static readonly float speedStep = 2f;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        moveSpeed = defaultMoveSpeed;
        DontDestroyOnLoad(gameObject);
        // TODO: uiManager.maxPossibleSpeed 설정해주기
    }

    public void LevelUp(int levelIncrement = 1)
    {
        level += levelIncrement;
        if (level > maxLevel)
        {
            level = maxLevel;
        }
        moveSpeed = GetSpeed(level);
        GameManager.Instance.uiManager.UpdateLevel(level);
        // TODO: uiManager.SetPlayerMaxSpeed로 스피드별 최대 속도 건네주기

    }

    public void SlowPlayer(float slowRate)
    {
        if (!isSlow)
        {
            isSlow = true;
            DebufManager.Instance.UpdateDebufText(DebufType.Stun);
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
            DebufManager.Instance.UpdateDebufText(DebufType.None);
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


    public void IncreaseSpeed()
    {
        moveSpeed = Mathf.Min(moveSpeed + speedStep, maxMoveSpeed);
    }

    public void DecreaseSpeed()
    {
        moveSpeed = Mathf.Max(moveSpeed - speedStep, minMoveSpeed);
    }

    private float GetSpeed(int level)
    {
        return Mathf.Clamp(defaultMoveSpeed * Mathf.Pow(speedGrowthRate, level - 1), minMoveSpeed, maxMoveSpeed);
    }

}
