using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncounterEvent : MonoBehaviour
{

    private BoxCollider roomCollider;
    private Vector3 roomCenter;
    private float roomXSize;
    private float roomZSize;
    private bool isEncountered = false;

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

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isEncountered)
        {
            isEncountered = true; // Set the flag to true to prevent multiple spawns
            StartCoroutine(SpawnCoroutine());
        }
    }

    public IEnumerator SpawnCoroutine()
    {
        Vector3 cameraFocusPosition = roomCenter + new Vector3(0, 30, -100);
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
        TitleManager.Instance.ShowSubtitle(roomDescription, Color.white, FlashPreset.StandardFlash);
        yield return new WaitForSecondsRealtime(0.5f); // Wait for a moment after spawning
    }
}
