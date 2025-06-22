using System.Collections;
using UnityEngine;

public class PythonMonster : MonoBehaviour, IMonsterController
{
    public AudioClip attackSound; // Sound effect for the attack

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

    public void SetLevel(int level)
    {
        // Example growth rates, adjust as needed
        float slowRateGrowth = 0.05f;
        float slowDurationGrowth = 0.1f;
        float reverseDurationGrowth = 0.1f;

        slowRate += slowRate * slowRateGrowth * level;
        slowDuration += slowDuration * slowDurationGrowth * level;
        reverseDuration += reverseDuration * reverseDurationGrowth * level;
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
            SoundEffectManager.Instance.PlayOneShotOnce(attackSound); // Play attack sound effect

            // 3. Destroy the monster after applying the debuff
            // TODO: Add a sound effect, particle effect
            Destroy(gameObject);
        }
    }

    public void OnAttack()
    {
        // PythonMonster does not have an attack method
        Debug.LogWarning("[PythonMonster] Attack method is not implemented.");
    }

    public void EndMonster()
    {
        // PythonMonster does not have an end method
        Debug.LogWarning("[PythonMonster] EndMonster method is not implemented.");
        Destroy(gameObject);
    }
}