using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallManager : MonoBehaviour
{
    public static BallManager Instance { get; private set; }
    public List<GameObject> balls; // List to hold all the balls

    private Dictionary<BallType, GameObject> ballPrefabs; // Dictionary to hold ball prefabs by type

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
        DontDestroyOnLoad(gameObject); // 선택사항: 씬 전환 시 유지하고 싶을 때

        if (balls == null || balls.Count != 7)
        {
            Debug.LogError("Ball list is not initialized or empty!");
            return;
        }

        ballPrefabs = new Dictionary<BallType, GameObject>
        {
            { BallType.Football, balls[0] },
            { BallType.Basketball, balls[1] },
            { BallType.AmericanFootball, balls[2] },
            { BallType.Volleyball, balls[3] },
            { BallType.BeachBall, balls[4] },
            { BallType.BowlingBall, balls[5] },
            { BallType.TennisBall, balls[6] }
        };
    }

    public GameObject GetBallPrefab(BallType type)
    {
        if (ballPrefabs.TryGetValue(type, out GameObject prefab))
        {
            return prefab;
        }
        else
        {
            Debug.LogError($"Ball type {type} not found in the dictionary!");
            return null;
        }
    }

    public string GetBallTrigger(BallType type)
    {
        switch (type)
        {
            case BallType.Football:
                return "power_throw_trig";
            case BallType.Basketball:
                return "power_throw_trig";
            case BallType.AmericanFootball:
                return "big_throw_trig";
            case BallType.Volleyball:
                return "big_throw_trig";
            case BallType.BeachBall:
                return "big_throw_trig";
            case BallType.BowlingBall:
                return "bowling_ball_trig";
            case BallType.TennisBall:
                return "small_throw_trig";
            default:
                Debug.LogError($"No trigger defined for ball type {type}");
                return null;
        }
    }
}
