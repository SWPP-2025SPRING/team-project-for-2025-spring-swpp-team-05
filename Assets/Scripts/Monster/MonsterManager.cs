using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    public GameObject reportMonsterPrefab;
    public GameObject professorMonsterPrefab;
    public GameObject pythonMonsterPrefab;
    public GameObject peMonsterPrefab;

    public AudioClip reportMonsterSound;
    public AudioClip professorMonsterSound;
    public AudioClip pythonMonsterSound;
    public AudioClip peMonsterSound;

    public AudioClip clearSound;

    public static MonsterManager Instance { get; private set; }

    // !!! for integration test: dictionary override
    private Dictionary<MonsterType, GameObject> monsterPrefabDict = new();
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public GameObject GetMonsterPrefab(MonsterType type)
    {
        // 1. 테스트 딕셔너리에 있으면 그걸 먼저 반환
        if (monsterPrefabDict.ContainsKey(type))
        {
            return monsterPrefabDict[type];
        }

        // 2. 아니면 원래 프리팹 반환
        return type switch
        {
            MonsterType.Report => reportMonsterPrefab,
            MonsterType.Professor => professorMonsterPrefab,
            MonsterType.Python => pythonMonsterPrefab,
            MonsterType.PE => peMonsterPrefab,
            _ => null,
        };

    }

    public string GetMonsterName(MonsterType type)
    {
        return type switch
        {
            MonsterType.Report => "Report Monster",
            MonsterType.Professor => "Professor Monster",
            MonsterType.Python => "Python Monster",
            MonsterType.PE => "PE Monster",
            _ => "Unknown Monster",
        };
    }

    // !!! for integration test: dummy prefab override
    public void OverrideMonsterPrefab(MonsterType type, GameObject dummy)
    {
        monsterPrefabDict[type] = dummy;
    }

    public AudioClip GetMonsterSound(MonsterType type)
    {
        switch (type)
        {
            case MonsterType.Report:
                return reportMonsterSound;
            case MonsterType.Professor:
                return professorMonsterSound;
            case MonsterType.Python:
                return pythonMonsterSound;
            case MonsterType.PE:
                return peMonsterSound;
            default:
                return null;
        }
    }
}