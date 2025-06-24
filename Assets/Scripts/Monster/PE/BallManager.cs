using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallManager : MonoBehaviour
{
    public static BallManager Instance { get; private set; }
    public List<GameObject> balls; // List to hold all the balls
    public GameObject defaultBall;

    void Awake()
    {
        // ✅ 싱글톤 인스턴스 설정
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Multiple BallManager instances found! Destroying duplicate.");
            Destroy(gameObject); // 중복 방지
            return;
        }

        Instance = this;
    }

    public GameObject GetBallPrefab(BallType type)
    {
        if (balls != null && (int)type >= 0 && (int)type < balls.Count && balls[(int)type] != null)
        {
            return balls[(int)type];
        }
        else
        {
            Debug.LogWarning($"Ball at index {type} is missing or null. Using defaultBallPrefab.");
            return defaultBall;
        }
    }

    public string GetBallTrigger(BallType type)
    {
        switch (type)
        {
            case BallType.Football:
                return "kick_trig";
            case BallType.Basketball:
                return "big_throw_trig";
            case BallType.AmericanFootball:
                return "big_throw_trig";
            case BallType.Volleyball:
                return "big_throw_trig";
            case BallType.BeachBall:
                return "power_throw_trig";
            case BallType.BowlingBall:
                return "bowling_trig";
            case BallType.TennisBall:
                return "small_throw_trig";
            default:
                Debug.LogError($"No trigger defined for ball type {type}");
                return null;
        }
    }
}
