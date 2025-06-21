using NUnit.Framework;
using UnityEngine;

public class PlayerStatusTests
{
    private GameObject go;
    private PlayerStatus status;

    [SetUp]
    public void Setup()
    {
        go = new GameObject("TestPlayer");
        status = go.AddComponent<PlayerStatus>();
        status.SendMessage("Awake");  // Singleton 초기화 및 기본값 설정
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(go);
        typeof(PlayerStatus).GetField("instance", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic).SetValue(null, null);
    }

    [Test]
    public void IncreaseAttackPower_ClampsAtMax()
    {
        for (int i = 0; i < 200; i++) status.IncreaseAttackPower();
        Assert.AreEqual(PlayerStatus.maxAttackPower, status.attackPower, 0.01f);
    }

    [Test]
    public void IncreaseAttackRange_ClampsAtMax()
    {
        for (int i = 0; i < 200; i++) status.IncreaseAttackRange();
        Assert.AreEqual(PlayerStatus.maxAttackRange, status.attackRange, 0.01f);
    }

    [Test]
    public void IncreaseSpeed_ClampsAtMax()
    {
        for (int i = 0; i < 100; i++) status.IncreaseSpeed();
        Assert.LessOrEqual(status.moveSpeed, PlayerStatus.maxMoveSpeed + 0.01f);
    }

    [Test]
    public void DecreaseSpeed_ClampsAtMin()
    {
        for (int i = 0; i < 100; i++) status.DecreaseSpeed();
        Assert.GreaterOrEqual(status.moveSpeed, PlayerStatus.minMoveSpeed - 0.01f);
    }

    [Test]
    public void SlowPlayer_ReducesSpeedAndSetsIsSlow()
    {
        status.SlowPlayer(0.3f);
        Assert.IsTrue(status.isSlow);
        Assert.AreEqual(status.defaultMoveSpeed * 0.7f, status.moveSpeed, 0.01f);
    }

    [Test]
    public void ReviveSlow_RecoversSpeedAndUnsetsIsSlow()
    {
        status.SlowPlayer(0.5f);
        status.ReviveSlow(0.5f);
        Assert.IsFalse(status.isSlow);
        Assert.AreEqual(status.defaultMoveSpeed, status.moveSpeed, 0.01f);
    }

    [Test]
    public void GetAttackPowerRatio_IsNormalized()
    {
        status.IncreaseAttackPower(); // 기본값은 10, 증가하면 11
        float ratio = status.GetAttackPowerRatio();
        Assert.IsTrue(ratio > 0f && ratio <= 1f);
    }
}