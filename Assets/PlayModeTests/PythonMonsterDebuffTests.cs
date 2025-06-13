using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;

public class PythonMonsterDebuffTests
{
    private GameObject playerObj;
    private PlayerStatus playerStatus;
    private DebuffHandler debuffHandler;
    private GameObject monsterObj;
    private PythonMonster pythonMonster;

    [SetUp]
    public void Setup()
    {
        // 플레이어 오브젝트 및 컴포넌트 준비
        playerObj = new GameObject("Player");
        playerObj.tag = "Player";
        playerStatus = playerObj.AddComponent<PlayerStatus>();
        debuffHandler = playerObj.AddComponent<DebuffHandler>();
        playerObj.AddComponent<BoxCollider>().isTrigger = true;
        playerObj.AddComponent<Rigidbody>().isKinematic = true;

        playerStatus.defaultMoveSpeed = 10f;
        playerStatus.moveSpeed = 10f;

        // 몬스터 오브젝트 및 컴포넌트 준비
        monsterObj = new GameObject("PythonMonster");
        pythonMonster = monsterObj.AddComponent<PythonMonster>();
        monsterObj.AddComponent<BoxCollider>().isTrigger = true;
        pythonMonster.slowRate = 0.5f;
        pythonMonster.slowDuration = 1.0f;
        pythonMonster.reverseDuration = 1.0f;

        // 초기 위치 분리
        playerObj.transform.position = Vector3.zero;
        monsterObj.transform.position = Vector3.one * 10f;
    }

    [TearDown]
    public void Teardown()
    {
        Object.DestroyImmediate(playerObj);
        Object.DestroyImmediate(monsterObj);
    }

    [UnityTest]
    public IEnumerator PythonMonster_Applies_SpeedDebuff()
    {
        // 랜덤 우회: SpeedDebuff 강제 적용
        // PythonMonster의 rand 변수가 private이면, 생성자나 테스트 전용 메서드로 제어하세요.
        // 여기서는 실제 랜덤 대신 항상 SpeedDebuff가 적용되도록 PythonMonster 코드를 약간 수정해야 할 수 있습니다.
        // 또는, PythonMonster의 OnTriggerEnter를 직접 호출하는 방식으로도 테스트 가능

        // 몬스터를 플레이어 위치로 이동시켜 충돌 유도
        monsterObj.transform.position = Vector3.zero;
        yield return new WaitForFixedUpdate();

        // SpeedDebuff 적용 확인
        Assert.Less(playerStatus.moveSpeed, playerStatus.defaultMoveSpeed, "SpeedDebuff 적용 실패");

        // 지속시간 후 복구 확인
        yield return new WaitForSeconds(pythonMonster.slowDuration + 0.1f);
        Assert.AreEqual(playerStatus.defaultMoveSpeed, playerStatus.moveSpeed, 0.01f, "SpeedDebuff 복구 실패");
    }

    [UnityTest]
    public IEnumerator PythonMonster_Applies_ReverseControlDebuff()
    {
        // 테스트 신뢰성을 위해 PythonMonster 코드를 수정해 항상 ReverseControlDebuff가 적용되게 하거나,
        // OnTriggerEnter를 직접 호출하여 ReverseControlDebuff를 강제로 적용하세요.

        // 몬스터를 플레이어 위치로 이동시켜 충돌 유도
        monsterObj.transform.position = Vector3.zero;
        yield return new WaitForFixedUpdate();

        // ReverseControlDebuff 적용 확인
        Assert.IsTrue(playerStatus.isReverseControl, "ReverseControlDebuff 적용 실패");

        // 지속시간 후 복구 확인
        yield return new WaitForSeconds(pythonMonster.reverseDuration + 0.1f);
        Assert.IsFalse(playerStatus.isReverseControl, "ReverseControlDebuff 복구 실패");
    }
}
