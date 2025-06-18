using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrels : MonoBehaviour
{
    public static Barrels Instance { get; private set; }  // 진짜 싱글톤 속성
    public GameObject[] barrels; // Array of barrel prefabs

    private void Awake()
    {
        // 싱글톤 중복 방지
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Duplicate Barrels instance found. Destroying the new one.");
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // 필요 시 DontDestroyOnLoad 설정
        // DontDestroyOnLoad(gameObject);
    }

    public int GetBarrelCount()
    {
        Debug.Log($"Barrel count: {barrels?.Length ?? 0}");
        return barrels?.Length ?? 0;
    }

    public GameObject GetBarrelPrefab(int index)
    {
        if (barrels == null || barrels.Length == 0)
        {
            Debug.LogError("Barrel prefab array is not set.");
            return null;
        }

        if (index >= 0 && index < barrels.Length)
        {
            return barrels[index];
        }
        else
        {
            Debug.LogError($"Barrel index {index} out of bounds (0 to {barrels.Length - 1}).");
            return null;
        }
    }
}
