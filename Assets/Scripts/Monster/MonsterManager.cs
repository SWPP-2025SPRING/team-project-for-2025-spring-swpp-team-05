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
        switch (type)
        {
            case MonsterType.Report:
                return reportMonsterPrefab;
            case MonsterType.Professor:
                return professorMonsterPrefab;
            case MonsterType.Python:
                return pythonMonsterPrefab;
            case MonsterType.PE:
                return peMonsterPrefab;
            default:
                return null;
        }
    }

    public string GetMonsterName(MonsterType type)
    {
        switch (type)
        {
            case MonsterType.Report:
                return "Report Monster";
            case MonsterType.Professor:
                return "Professor Monster";
            case MonsterType.Python:
                return "Python Monster";
            case MonsterType.PE:
                return "PE Monster";
            default:
                return "Unknown Monster";
        }
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