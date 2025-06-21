using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainColliderManager : MonoBehaviour
{
    public float trackInterval = 3.0f;
    private Vector3 lastPosition;
    private float trackTimer;
    // Start is called before the first frame update
    void Start()
    {
        lastPosition = transform.position;
        trackTimer = trackInterval; // Initialize the timer
    }

    // Update is called once per frame
    void Update()
    {
        if (trackTimer < trackInterval)
        {
            trackTimer += Time.deltaTime;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Terrain"))
        {
            transform.position = lastPosition; // Reset position to last road position
            PlayerStatus.instance.SetSpeedZero();
            trackTimer = 0f; // Reset the timer
        }
        if (trackTimer < trackInterval)
        {
            return;
        }

        if (collision.gameObject.CompareTag("Road"))
        {
            lastPosition = transform.position;
            trackTimer = 0f; // Reset the timer
        }
    }
}
