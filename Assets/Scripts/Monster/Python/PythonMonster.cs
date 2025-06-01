using System.Collections;
using UnityEngine;

public class PythonMonster : MonoBehaviour
{
    public float slowRate = 0.3f;
    public float slowDuration = 5f;
    public float reverseDuration = 3f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            int rand = Random.Range(0, 2);
            IDebuff debuffToApply;

            if (rand == 0)
            {
                debuffToApply = new SpeedDebuff(slowRate, slowDuration);
            }
            else
            {
                debuffToApply = new ReverseControlDebuff(reverseDuration);
            }

            DebuffHandler handler = other.GetComponent<DebuffHandler>();
            if (handler != null)
            {
                handler.ApplyDebuff(debuffToApply);
            }
            else
            {
                Debug.LogWarning("[PythonMonster] No DebuffHandler exists!");
            }
        }
    }
}