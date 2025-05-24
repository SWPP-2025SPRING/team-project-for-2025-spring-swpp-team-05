using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public static PlayerStatus instance { get; private set; }

    public int moveSpeed { get; private set; }
    public int attackPower { get; private set; }
    public float attackRange { get; private set; }

    public float defaultMoveSpeed = 10.0f;
    public int defaultAttackPower = 10;
    public float defaultAttackRange = 3.0f;

    private void stunRateAgg = 0f;
    public bool isStun { get; private set; } = false;

    public int level { get; private set; } = 1;
    public int exp { get; private set; } = 0;
    public int nextExp { get; private set; } = 100;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return
        }
        instance = this;
        moveSpeed = defaultMoveSpeed;
        attackPower = defaultAttackPower;
        attackRange = defaultAttackRange;
        DontDestroyOnLoad(gameObject);
    }

    public void StunPlayer(float stunRate)
    {
        if (!isStun)
        {
            isStun = true;
        }
        stunRateAgg += stunRate;
        moveSpeed = defaultMoveSpeed * (1 - stunRateAgg);
        if (moveSpeed < 0)
        {
            moveSpeed = 0.01f;
        }
    }

    public void ReviveStun(float stunRate, bool isFinal)
    {
        stunRateAgg -= stunRate;
        if (stunRateAgg < 0)
        {
            stunRateAgg = 0;
        }
        moveSpeed = defaultMoveSpeed * (1 - stunRateAgg);
        if (moveSpeed < 0)
        {
            moveSpeed = 0.01f;
        }
        if (isFinal)
        {
            isStun = false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
