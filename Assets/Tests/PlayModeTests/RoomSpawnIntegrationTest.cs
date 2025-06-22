using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Reflection;

public class RoomSpawnIntegrationTest
{
    private GameObject roomGO;
    private GameObject monsterManagerGO;
    private GameObject titleManagerGO;
    private GameObject dummyPrefab;

    [UnityTest]
    public IEnumerator Player_EntersRoom_StaticSpawn_TwoMonstersSpawned()
    {
        // Dummy Monster 프리팹 생성
        dummyPrefab = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        dummyPrefab.name = "MockMonster";
        dummyPrefab.SetActive(true); // 반드시 활성화된 상태여야 생성 후 바로 보임

        // MonsterManager 설정
        monsterManagerGO = new GameObject("MonsterManager");
        var monsterManager = monsterManagerGO.AddComponent<MonsterManager>();
        typeof(MonsterManager)
            .GetField("Instance", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public)
            ?.SetValue(null, monsterManager);
        MonsterManager.Instance.OverrideMonsterPrefab(MonsterType.Report, dummyPrefab);

        // TitleManager 설정 (필수 텍스트들 더미 생성)
        titleManagerGO = new GameObject("TitleManager");
        var tm = titleManagerGO.AddComponent<TitleManager>();
        tm.titleText = DummyText(); tm.subtitleText = DummyText(); tm.eventText = DummyText();
        tm.levelText = DummyText(); tm.amountText = DummyText();

        // Room 및 RoomSpawnManager 설정
        roomGO = new GameObject("Room");
        roomGO.transform.position = Vector3.zero;
        var room = roomGO.AddComponent<RoomSpawnManager>();
        var collider = roomGO.AddComponent<BoxCollider>();
        collider.isTrigger = true;

        room.roomLevel = 1;
        room.isStaticSpawn = true;
        room.monsterType = MonsterType.Report;
        room.staticSpawnPositions = new Vector3[] {
            new Vector3(0,0,0), new Vector3(1,0,0)
        };

        // private 필드 주입
        typeof(RoomSpawnManager).GetField("playerPosition", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.SetValue(room, Vector3.back);
        typeof(RoomSpawnManager).GetField("isSpawned", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.SetValue(room, true);

        // 호출
        room.SpawnMonstersAtFixedPositions();
        yield return null;

        // 자식 오브젝트 수 확인
        int childCount = roomGO.transform.childCount;
        Assert.AreEqual(2, childCount);
    }

    private TMPro.TextMeshProUGUI DummyText()
    {
        return new GameObject().AddComponent<TMPro.TextMeshProUGUI>();
    }

    [UnityTest]
    public IEnumerator ProperMonsterAmount_ReturnsExpectedValue()
    {
        var roomGO = new GameObject("TestRoom");
        var roomManager = roomGO.AddComponent<RoomSpawnManager>();
        roomManager.roomLevel = 3;
        roomManager.monsterCoefficient = 2;

        // private static float growthRate = 0.2f
        MethodInfo method = typeof(RoomSpawnManager).GetMethod("ProperMonsterAmount", BindingFlags.Instance | BindingFlags.Public);
        int amount = (int)method.Invoke(roomManager, null);

        float expected = Mathf.Ceil(2f * Mathf.Exp(3 * 0.2f));
        Assert.AreEqual((int)expected, amount);

        Object.DestroyImmediate(roomGO);
        yield return null;
    }

    [UnityTest]
    public IEnumerator SummonPosition_WithinRoomBounds()
    {
        var roomGO = new GameObject("TestRoom");
        var collider = roomGO.AddComponent<BoxCollider>();
        collider.size = new Vector3(10f, 5f, 10f);
        collider.center = Vector3.zero;
        var roomManager = roomGO.AddComponent<RoomSpawnManager>();
        roomGO.transform.position = Vector3.zero;

        typeof(RoomSpawnManager).GetField("roomCollider", BindingFlags.Instance | BindingFlags.NonPublic)
            ?.SetValue(roomManager, collider);
        typeof(RoomSpawnManager).GetField("roomXSize", BindingFlags.Instance | BindingFlags.NonPublic)
            ?.SetValue(roomManager, 10f);
        typeof(RoomSpawnManager).GetField("roomZSize", BindingFlags.Instance | BindingFlags.NonPublic)
            ?.SetValue(roomManager, 10f);
        typeof(RoomSpawnManager).GetField("roomCenter", BindingFlags.Instance | BindingFlags.NonPublic)
            ?.SetValue(roomManager, Vector3.zero);

        var method = typeof(RoomSpawnManager).GetMethod("summonPosition", BindingFlags.Instance | BindingFlags.NonPublic);
        Vector3 pos = (Vector3)method.Invoke(roomManager, new object[] { 0, 2, 2 });

        Assert.IsTrue(Mathf.Abs(pos.x) <= 5f && Mathf.Abs(pos.z) <= 5f);

        Object.DestroyImmediate(roomGO);
        yield return null;
    }

    [TearDown]
    public void Teardown()
    {
        Object.DestroyImmediate(roomGO);
        Object.DestroyImmediate(monsterManagerGO);
        Object.DestroyImmediate(titleManagerGO);
        Object.DestroyImmediate(dummyPrefab);

        foreach (var go in GameObject.FindObjectsOfType<GameObject>())
        {
            if (go.name.StartsWith("MockMonster") || go.name.Contains("Monster"))
                Object.DestroyImmediate(go);
        }
    }
}
