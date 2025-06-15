using System.Collections;
using UnityEngine;

public class PythonMonster : MonoBehaviour, IMonsterController
{
    public float slowRate = 0.3f;
    public float slowDuration = 5f;
    public float reverseDuration = 3f;

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