using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDebuffManager : MonoBehaviour
{
    private float baseSpeed;
    private bool isReverseControl = false;
    private List<Debuff> activeDebuffs = new List<Debuff>();

    void Start()
    {
        baseSpeed = GetComponent<PlayerController>().moveSpeed;
    }

    void Update()
    {
        for (int i = activeDebuffs.Count - 1; i >= 0; i--)
        {
            activeDebuffs[i].Timer += Time.deltaTime;
            if (activeDebuffs[i].Timer >= activeDebuffs[i].Duration)
            {
                RemoveDebuff(activeDebuffs[i]);
                activeDebuffs.RemoveAt(i);
            }
        }
    }

    public void ApplyDebuff(DebuffType type, float duration)
    {
        // 같은 타입의 디버프가 이미 있는지 확인
        Debuff existingDebuff = activeDebuffs.Find(d => d.Type == type);
        if (existingDebuff != null)
        {
            existingDebuff.Timer = 0f;
            existingDebuff.Duration = duration;
        }
        else
        {
            Debuff newDebuff = new Debuff(type, duration);
            activeDebuffs.Add(newDebuff);

            switch (type)
            {
                case DebuffType.SpeedDown:
                    GetComponent<PlayerController>().moveSpeed *= 0.5f;
                    break;
                case DebuffType.ReverseControl:
                    isReverseControl = true;
                    break;
            }
        }
    }

    private void RemoveDebuff(Debuff debuff)
    {
        switch (debuff.Type)
        {
            case DebuffType.SpeedDown:
                GetComponent<PlayerController>().moveSpeed = baseSpeed;
                break;
            case DebuffType.ReverseControl:
                isReverseControl = false;
                break;
        }
    }

}