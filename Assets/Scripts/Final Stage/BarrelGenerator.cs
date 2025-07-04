using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelGenerator : MonoBehaviour
{
    public float spawnInterval = 2f; // Time interval between spawns
    public float yOffset = 0.5f; // Y offset for spawn position
    public int summonCount = 3; // Number of barrels

    private float coolTime = 0f; // Time for the next spawn
    private Vector3[] spawnPositions; // Array of spawn positions
    private bool isXMajor = false; // Flag to check if the X axis is major
    private bool isPositiveDirection = false; // Flag to check if the direction is positive

    private List<GameObject> spawnedBarrels = new List<GameObject>(); // List to keep track of spawned barrels
    // Start is called before the first frame update
    void Start()
    {
        BoxCollider boxCollider = GetComponent<BoxCollider>();
        if (boxCollider != null)
        {
            // 월드 공간에서 콜라이더 중심 계산
            Vector3 center = transform.TransformPoint(boxCollider.center);
            Vector3 size = Vector3.Scale(boxCollider.size, transform.lossyScale);
            Vector3 forward = transform.forward; // Get the forward direction of the object
            spawnPositions = new Vector3[summonCount];

            isXMajor = Mathf.Abs(forward.x) > Mathf.Abs(forward.z);
            isPositiveDirection = (isXMajor && forward.x > 0) || (!isXMajor && forward.z > 0);

            Vector3 margin = transform.forward * 3 * (Vector3.Dot(forward, transform.forward) > 0 ? -1 : 1);
            Vector3 startPoint = center - (transform.right * (size.x / 2f)) + (transform.forward * (size.z / 2f * (Vector3.Dot(forward, transform.forward) > 0 ? -1 : 1))) - margin;

            for (int i = 0; i < summonCount; i++)
            {
                float spacing = size.x / summonCount;
                Vector3 offset = transform.right * spacing * i;
                Vector3 spawnPos = startPoint + offset + Vector3.up * yOffset;
                spawnPositions[i] = spawnPos;
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        coolTime += Time.deltaTime; // Increment the cooldown time
        if (coolTime >= spawnInterval)
        {
            coolTime = 0f; // Reset the cooldown time
            int[] barrelIndex = new int[summonCount]; // Array to store random barrel indices
            for (int i = 0; i < summonCount; i++)
            {
                bool empty = Random.Range(0f, 1f) < 0.8f; // 20% chance to skip summoning a barrel
                if (empty)
                {
                    barrelIndex[i] = -1; // Set to -1 to indicate no barrel
                }
                else
                {
                    int randomIndex = Random.Range(0, Barrels.Instance.GetBarrelCount()); // Get a random barrel index
                    barrelIndex[i] = randomIndex; // Store the index in the array
                }
            }
            SummonBarrel(barrelIndex); // Call the method to summon barrels
        }
    }

    void SummonBarrel(int[] barrelIndex)
    {
        for (int i = 0; i < summonCount; i++)
        {
            if (barrelIndex[i] == -1)
                continue; // Skip if the index is -1

            // 목표: barrel의 local right == 부모의 forward
            Vector3 forward = transform.forward;
            Vector3 up = Vector3.up;
            Quaternion rotation = Quaternion.LookRotation(Vector3.Cross(up, forward), up);

            GameObject barrel = Instantiate(Barrels.Instance.GetBarrelPrefab(barrelIndex[i]), spawnPositions[i], rotation);
            barrel.transform.parent = transform; // Set the parent of the barrel to this object
            spawnedBarrels.Add(barrel); // Add the barrel to the list of spawned barrels
        }
    }
}
