using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct DebuffChance
{
    public DebuffType type;
    public float probability;
}

public class PythonManager : MonoBehaviour
{
    public DebuffChance[] debuffChances;
    public float debuffDuration;

    [SerializeField] private bool useDebug = false;

    DebuffType GetRandomDebuffType()
    {
        float rand = UnityEngine.Random.value;
        float cumulative = 0f;

        foreach (var debuff in debuffChances)
        {
            cumulative += debuff.probability;
            if (rand < cumulative)
                if (useDebug)
                    Debug.Log($"[PythonMonster] 랜덤값: {rand:f3}, 누적확률: {cumulative: F3}, 선택: {debuff.type}");
            return debuff.type;
        }

        return debuffChances[debuffChances.Length - 1].type;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            DebuffType selectedType = GetRandomDebuffType();
            PlayerDebuffManager player = collision.gameObject.GetComponent<PlayerDebuffManager>();
            if (player != null)
                player.ApplyDebuff(selectedType, debuffDuration);
        }
    }
}