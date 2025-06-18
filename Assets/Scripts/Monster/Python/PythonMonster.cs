using System.Collections;
using UnityEngine;

public class PythonMonster : MonoBehaviour
{

    public float moveSpeed = 5f;
    public float moveRange = 3f;

    private Vector3 startPosition;

    public float slowRate = 0.3f;
    public float slowDuration = 5f;
    public float reverseDuration = 3f;
    public float cameraTiltDuration = 5f;

    public float cameraFlipDuration = 5f;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        float offsetX = Mathf.PingPong(Time.time * moveSpeed, moveRange * 2) - moveRange;
        transform.position = startPosition + transform.right * offsetX;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 1. Randomly select a debuff to apply
            int rand = Random.Range(0, 4);
            IDebuff debuffToApply;

            if (rand == 0)
            {
                debuffToApply = new SpeedDebuff(slowRate, slowDuration);
            }
            else if (rand == 1)
            {
                debuffToApply = new ReverseControlDebuff(reverseDuration);
            }
            else if (rand == 2)
            {
                debuffToApply = new CameraTiltDebuff(cameraTiltDuration);
            }
            else
            {
                debuffToApply = new CameraFlipDebuff(cameraFlipDuration);
            }


            // 2. Apply the debuff to the Player
            DebuffHandler handler = other.GetComponent<DebuffHandler>();

            if (handler != null)
            {
                handler.ApplyDebuff(debuffToApply);
            }
            else
            {
                Debug.LogWarning("DebuffHandler component not found on Player.");
            }

            // 3. Destroy the monster after applying the debuff
            // TODO: Add a sound effect, particle effect
            Destroy(gameObject);
        }
    }
}