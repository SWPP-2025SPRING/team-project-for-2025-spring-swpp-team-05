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
    public Vector3 yOffset = Vector3.zero; // Offset to apply to the y-coordinate of spawn positions

    private BoxCollider roomCollider;
    private Vector3 roomCenter;
    private float roomXSize;
    private float roomZSize;

    private int monsterCount;
    private int remainMonsterCount;

    private MonsterFactory monsterFactory;
    private bool isSpawned = false;
    private bool isCleared = false;
    private bool isOnRoom = false;

    private Vector3 playerPosition = Vector3.zero; // Player's position for debugging


    [Header("About Room")]
    public string roomName = "Default Room"; // Name of the room for debugging
    public string roomDescription = "This is a default room for monster spawning."; // Description of the room for debugging

    // Start is called before the first frame update
    void Start()
    {
        roomCollider = GetComponent<BoxCollider>();
        roomCenter = transform.position + transform.rotation * Vector3.Scale(roomCollider.center, transform.lossyScale);
        roomCenter.y = transform.position.y;
        roomXSize = roomCollider.size.x;
        roomZSize = roomCollider.size.z;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isOnRoom = true; // Set the flag to true when player enters the room
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isOnRoom = false;
            if (!isCleared)
            {
                monsterFactory?.DestroyMonsters(); // Clean up monsters when exiting
                TitleManager.Instance.HideRoomText();
            }
            if (monsterType == MonsterType.Report)
            {
                PlayerControl playerControl = other.GetComponent<PlayerControl>();
                if (playerControl != null)
                {
                    HandleReportExit(playerControl); // Reset code factory when exiting report
                }
            }
        }
    }
    public void HandleEnter(Collider other)
    {
        if (!isCleared && !isOnRoom)
        {
            playerPosition = other.transform.position; // Store player's position for debugging
            isSpawned = true; // Set the flag to true to prevent multiple spawns
            isOnRoom = true;
            if (monsterType != MonsterType.None)
            {
                StartCoroutine(SpawnCoroutine());
            }
        }
        else if (isOnRoom) // 입구로 나가려고함
        {
            TitleManager.Instance.ShowEventText("입구로 나갈 수 없습니다.", Color.white, FlashPreset.StandardFlash);
        }
        else
        {
            TitleManager.Instance.ShowEventText("이 방은 이미 클리어되었습니다.", Color.white, FlashPreset.StandardFlash);
        }
    }

    public void HandleExit(Collider other)
    {
        if (isOnRoom && !isCleared)
        {
            isCleared = true;
            isOnRoom = false; // Reset the flag when exiting
            monsterFactory?.DestroyMonsters();
            if (roomLevel > 1)
            {
                PlayerStatus.instance.LevelUp(monsterType, roomLevel - 1);
            }
            if (monsterType == MonsterType.Report)
            {
                HandleReportExit(other.GetComponent<PlayerControl>());
            }
            TitleManager.Instance.HideRoomText();
        }
        else if (!isOnRoom) // 출구로 들어올려함
        {
            TitleManager.Instance.ShowEventText("출구로 들어올 수 없습니다.", Color.white, FlashPreset.StandardFlash);
        }
        else
        {
            TitleManager.Instance.ShowEventText("이 방은 이미 클리어되었습니다.", Color.white, FlashPreset.StandardFlash);
        }
    }



    public IEnumerator SpawnCoroutine()
    {
        Vector3 cameraFocusPosition = roomCenter + new Vector3(0, 30, 30);
        GameObject target = gameObject;
        IEnumerator spawnEvent = SpawnEventCoroutine();
        yield return StartCoroutine(CinematicCamera.Instance.StartCinematic(
            CinematicCamera.Instance.FocusCinematic(target, cameraFocusPosition, 2f, 2f, spawnEvent)
        ));
    }

    public IEnumerator SpawnEventCoroutine()
    {
        TitleManager.Instance.ShowTitle(roomName, Color.white, FlashPreset.StandardFlash);
        yield return new WaitForSecondsRealtime(0.5f); // Wait for a moment before spawning monsters
        if (isStaticSpawn)
        {
            SpawnMonstersAtFixedPositions();
        }
        else
        {
            SpawnMonsters();
        }
        SoundEffectManager.Instance.PlayOneShotOnce(MonsterManager.Instance.GetMonsterSound(monsterType));
        TitleManager.Instance.ShowSubtitle(roomDescription, Color.white, FlashPreset.StandardFlash);
        yield return new WaitForSecondsRealtime(0.5f); // Wait for a moment after spawning
    }

    public void SpawnMonsters()
    {
        monsterFactory = new MonsterFactory();
        monsterCount = ProperMonsterAmount();
        remainMonsterCount = monsterCount;
        TitleManager.Instance.ShowRoomText(MonsterManager.Instance.GetMonsterName(monsterType), monsterCount, roomLevel);

        float aspect = roomXSize / roomZSize;
        int col = Mathf.CeilToInt(Mathf.Sqrt(monsterCount * aspect));
        int row = Mathf.CeilToInt(monsterCount / col);

        for (int r = 0; r < row; r++)
        {
            for (int c = 0; c < col; c++)
            {
                int index = r * col + c;
                if (index >= monsterCount) break;

                Vector3 spawnPos = summonPosition(index, row, col) + yOffset;
                Quaternion spawnRot = Quaternion.Euler(0, Random.Range(0, 360), 0);
                GameObject monster = monsterFactory.CreateMonster(monsterType, roomLevel, spawnPos, spawnRot, transform);
            }
        }
    }

    public void SpawnMonstersAtFixedPositions()
    {
        monsterFactory = new MonsterFactory();
        remainMonsterCount = staticSpawnPositions.Length;
        TitleManager.Instance.ShowRoomText(MonsterManager.Instance.GetMonsterName(monsterType), remainMonsterCount, roomLevel);

        for (int i = 0; i < staticSpawnPositions.Length; i++)
        {
            Vector3 spawnPos = staticSpawnPositions[i] + roomCenter + yOffset;
            Vector3 direction = (playerPosition - spawnPos).normalized;
            Quaternion spawnRot = Quaternion.LookRotation(direction, Vector3.up);
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

        Vector3 basePos = new Vector3(
            roomCenter.x - roomXSize / 2 + cellWidth * (c + 0.5f),
            roomCenter.y + 2f,
            roomCenter.z - roomZSize / 2 + cellHeight * (r + 0.5f)
        );

        float xOffset = Random.Range(-cellWidth * 0.3f, cellWidth * 0.3f);
        float zOffset = Random.Range(-cellHeight * 0.3f, cellHeight * 0.3f);

        return new Vector3(
            basePos.x + xOffset,
            basePos.y,
            basePos.z + zOffset
        );
    }

    private void HandleReportExit(PlayerControl playerControl)
    {
        playerControl.ResetCodeFactory(); // Reset the code factory when exiting the report
    }
}