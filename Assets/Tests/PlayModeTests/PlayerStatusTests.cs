using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Reflection;

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

        var debufManager = new GameObject("DebufManager").AddComponent<DebufManager>();
        typeof(DebufManager).GetField("instance", BindingFlags.Static | BindingFlags.NonPublic)
            ?.SetValue(null, debufManager);
    }

    [TearDown]
    public void TearDown()
    {
        if (go != null)
        {
            Object.DestroyImmediate(go);
        }

        var field = typeof(PlayerStatus).GetField("instance",
            System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);

        if (field != null)
        {
            field.SetValue(null, null);  // Singleton 해제
        }
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



}