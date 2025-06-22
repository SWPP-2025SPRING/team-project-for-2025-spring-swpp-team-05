using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainColliderManager : MonoBehaviour
{
    public float trackInterval = 3.0f;
    private List<Vector3> lastPosition;
    private float trackTimer;
    public int maxStoredPositions = 10; // Maximum number of positions to store
    // Start is called before the first frame update
    void Start()
    {
        lastPosition = new List<Vector3>(maxStoredPositions);
        lastPosition.Add(transform.position); // Initialize with the current position
        trackTimer = trackInterval; // Initialize the timer
    }

    // Update is called once per frame
    void Update()
    {
        if (trackTimer < trackInterval)
        {
            trackTimer += Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (lastPosition.Count == 1)
            {
                Debug.LogWarning("No positions to reset to.");
                return;
            }
            lastPosition.RemoveAt(lastPosition.Count - 1); // Remove the last position
            transform.position = lastPosition[lastPosition.Count - 1]; // Reset to the new last position
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Terrain"))
        {
            TitleManager.Instance.ShowEventText("가속은 금물! 적당한 속도로 이동하세요.", Color.white, FlashPreset.StandardFlash);
            transform.position = lastPosition[lastPosition.Count - 1]; // Reset to the last stored position
            PlayerStatus.instance.SetSpeedZero();
            trackTimer = 0f; // Reset the timer
        }
        if (trackTimer < trackInterval)
        {
            return;
        }

        if (collision.gameObject.CompareTag("Road"))
        {
            lastPosition.Add(transform.position); // Store the current position
            if (lastPosition.Count > maxStoredPositions)
            {
                lastPosition.RemoveAt(0); // Remove the oldest position if we exceed the limit
            }
            trackTimer = 0f; // Reset the timer
        }
    }
}
