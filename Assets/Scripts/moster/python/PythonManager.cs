using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct DebuffChance
{
    public DebuffType type;
    public float probability;
}

public class PythonManager : MonoBehavior
{
    public DebuffChance[] debuffChances;

    [SerializedField] private bool useDebug = false;

    DebuffType GetRandomDebuffType()
    {
        float rand = Random.value;
        float cumulative = 0f;

        foreach (var debuff in debuffChances)
        {
            cumulative += debuff.probability;
            if (rand < cumulative)
                if (useDebug)
                    Debug.Log($"[PythonMonster] 랜덤값: {rand:f3}, 누적확률: {cumulative: F3}, 선택: {debuff.type}");
            return debuff.type;
        }

        if (useDebug)
            Debug.Log($"[PythonMonster] 랜덤값: {rand:f3}, 누적확률: {cumulative: F3}, 선택: {debuff.type}");
        return debuffChances[debuffChances.Length - 1].type;
    }

    void OnCollisionEnter(Collision collision)
    {
    }
}