using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawnManager : MonoBehaviour
{
    public GameObject roomPrefab;
    public int roomLevel = 2;
    public int monsterCoefficient = 2; // Monster Tier Coefficient

    private static float growthRate = 0.2f;

    private BoxCollider roomCollider;
    private Vector3 roomCenter;
    private float roomXSize;
    private float roomZSize;

    private int monsterCount;
    private int remainMonsterCount;

    private List<GameObject> summonedMonsters;

    // Start is called before the first frame update
    void Start()
    {
        roomCollider = GetComponent<BoxCollider>();
        roomCenter = transform.position + transform.rotation * Vector3.Scale(roomCollider.center, transform.lossyScale);
        roomCenter.y = transform.position.y;
        roomXSize = roomCollider.size.x;
        roomZSize = roomCollider.size.z;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SpawnMonsters();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DestroyMonsters();
        }
    }

    public void SpawnMonsters()
    {
        monsterCount = monsterAmount();
        remainMonsterCount = monsterCount;
        summonedMonsters = new List<GameObject>();
        
        float aspect = roomXSize / roomZSize;
        int col = Mathf.CeilToInt(Mathf.Sqrt(monsterCount * aspect));
        int row = Mathf.CeilToInt(monsterCount / col);

        for (int r = 0; r < row; r++)
        {
            for (int c = 0; c < col; c++)
            {
                int index = r * col + c;
                if (index >= monsterCount) break;

                Vector3 spawnPos = summonPosition(index, row, col);
                Quaternion spawnRot = Quaternion.Euler(0, Random.Range(0, 360), 0);
                GameObject monster = Instantiate(roomPrefab, spawnPos, spawnRot);
                monster.transform.SetParent(transform);
                summonedMonsters.Add(monster);
            }
        }
    }

    public void DestroyMonsters()
    {
        foreach (GameObject monster in summonedMonsters)
        {
            Destroy(monster);
        }
        summonedMonsters.Clear();
    }

    private int monsterAmount()
    {
        return Mathf.CeilToInt(monsterCoefficient * Mathf.Exp(roomLevel * growthRate));
    }

    private Vector3 summonPosition(int index, int row, int col)
    {
        int r = index / col;
        int c = index % col;

        float cellWidth = roomXSize / col;
        float cellHeight = roomZSize / row;

        // Base Positin
        Vector3 basePos = new Vector3(
            roomCenter.x - roomXSize / 2 + cellWidth * (c + 0.5f),
            roomCenter.y + 2f,
            roomCenter.z - roomZSize / 2 + cellHeight * (r + 0.5f)
        );


        // Random Offset
        float xOffset = Random.Range(-roomXSize / 3, roomXSize / 3);
        float zOffset = Random.Range(-roomZSize / 3, roomZSize / 3);
        return new Vector3(
            basePos.x + xOffset,
            basePos.y,
            basePos.z + zOffset
        );
    }
}
