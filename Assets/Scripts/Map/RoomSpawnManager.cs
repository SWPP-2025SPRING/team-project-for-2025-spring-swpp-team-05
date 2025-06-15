using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawnManager : MonoBehaviour
{
    [Header("Monster Spawn Settings")]
    public MonsterType monsterType = MonsterType.Report;

    [Header("Room Level Settings")]
    public int roomLevel = 2;
    public int monsterCoefficient = 2; // Monster Tier Coefficient
    public static float growthRate = 0.2f;

    [Header("Room Spawn Options")]
    public bool isStaticSpawn = true; // If true, monsters will spawn at fixed positions
    public Vector3[] staticSpawnPositions; // Fixed spawn positions for monsters

    private BoxCollider roomCollider;
    private Vector3 roomCenter;
    private float roomXSize;
    private float roomZSize;

    private int monsterCount;
    private int remainMonsterCount;

    private MonsterFactory monsterFactory;
    private bool isSpawned = false;

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
        if (other.CompareTag("Player") && !isSpawned)
        {
            isSpawned = true; // Set the flag to true to prevent multiple spawns
            StartCoroutine(SpawnCoroutine());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            monsterFactory?.DestroyMonsters();
            PlayerStatus.instance.LevelUp(roomLevel);
        }
    }

    public IEnumerator SpawnCoroutine()
    {
        Vector3 cameraFocusPosition = roomCenter + new Vector3(0, 20, 20);
        GameObject target = gameObject;
        IEnumerator spawnEvent = SpawnEventCoroutine();
        yield return StartCoroutine(CinematicCamera.Instance.StartCinematic(
            CinematicCamera.Instance.FocusCinematic(target, cameraFocusPosition, 2f, 2f, spawnEvent)
        ));
    }

    public IEnumerator SpawnEventCoroutine()
    {
        yield return new WaitForSecondsRealtime(0.5f); // Wait for a moment before spawning monsters
        if (isStaticSpawn)
        {
            SpawnMonstersAtFixedPositions();
        }
        else
        {
            SpawnMonsters();
        }
        yield return new WaitForSecondsRealtime(0.5f); // Wait for a moment after spawning
    }

    public void SpawnMonsters()
    {
        monsterFactory = new MonsterFactory();
        monsterCount = ProperMonsterAmount();
        remainMonsterCount = monsterCount;

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
                GameObject monster = monsterFactory.CreateMonster(monsterType, roomLevel, spawnPos, spawnRot, transform);
            }
        }
    }

    public void SpawnMonstersAtFixedPositions()
    {
        monsterFactory = new MonsterFactory();
        remainMonsterCount = staticSpawnPositions.Length;

        for (int i = 0; i < staticSpawnPositions.Length; i++)
        {
            Vector3 spawnPos = staticSpawnPositions[i] + roomCenter;
            Quaternion spawnRot = Quaternion.Euler(0, Random.Range(0, 360), 0);
            GameObject monster = monsterFactory.CreateMonster(monsterType, roomLevel, spawnPos, spawnRot, transform);
        }
    }

    public int ProperMonsterAmount()
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