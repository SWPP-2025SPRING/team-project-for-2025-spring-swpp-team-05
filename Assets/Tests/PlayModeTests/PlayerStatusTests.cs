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
        status.SendMessage("Awake");

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
            field.SetValue(null, null);
        }
    }

    [Test]
    public void IncreaseSpeed_ClampsAtMax()
    {
        for (int i = 0; i < 100; i++) status.IncreaseSpeed();
        Assert.LessOrEqual(status.moveSpeed, status.maxSpeed + 0.01f);
    }

    [Test]
    public void DecreaseSpeed_ClampsAtMin()
    {
        for (int i = 0; i < 100; i++) status.DecreaseSpeed();
        Assert.GreaterOrEqual(status.moveSpeed, status.minSpeed - 0.01f);
    }


    [Test]
    public void Accelerate_IncreasesMoveSpeed()
    {
        float before = status.moveSpeed;
        status.Accelerate(1f, 0.5f);
        Assert.GreaterOrEqual(status.moveSpeed, before);
    }

    [Test]
    public void DeAccelerate_DecreasesMoveSpeed()
    {
        status.IncreaseSpeed();
        float before = status.moveSpeed;
        status.DeAccelerate(1f, 0.5f);
        Assert.LessOrEqual(status.moveSpeed, before);
    }

    [Test]
    public void StopPlayer_SetsSpeedToZero()
    {
        status.IncreaseSpeed();
        status.StopPlayer();
        Assert.AreEqual(0f, status.moveSpeed, 0.01f);
    }

    [Test]
    public void ResumePlayer_AllowsSpeedChange()
    {
        status.StopPlayer();
        status.ResumePlayer();
        status.Accelerate(1f, 0.5f);
        Assert.Greater(status.moveSpeed, 0f);
    }
}
