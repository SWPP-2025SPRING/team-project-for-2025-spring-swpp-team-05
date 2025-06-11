using UnityEngine;
using System.Collections.Generic;

public class MonsterFactory
{
    private List<GameObject> summonedMonsters = new();
    public GameObject CreateMonster(
        MonsterType monster,
        int level,
        Vector3 position,
        Quaternion rotation,
        Transform parent
        )
    {
        GameObject monsterPrefab = MonsterManager.Instance.GetMonsterPrefab(monster);
        GameObject summonedMonster = Object.Instantiate(
            monsterPrefab,
            position,
            rotation,
            parent
        );
        IMonsterController monsterController = summonedMonster.GetComponent<IMonsterController>();
        monsterController?.SetLevel(level);
        summonedMonsters.Add(summonedMonster);
        return summonedMonster;
    }

    public void DestroyMonsters()
    {
        foreach (GameObject monster in summonedMonsters)
        {
            if (monster != null)
            {
                IMonsterController monsterController = monster.GetComponent<IMonsterController>();
                monsterController?.EndMonster();
                Object.Destroy(monster);
            }
        }
        summonedMonsters.Clear();
    }

    public int MonsterCount()
    {
        return summonedMonsters.Count;
    }
}