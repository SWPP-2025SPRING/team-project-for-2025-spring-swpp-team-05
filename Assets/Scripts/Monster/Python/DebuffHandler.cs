using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuffHandler : MonoBehaviour
{
    public class ActiveDebuff
    {
        private IDebuff debuff;
        private GameObject target;
        private float duration;
        private float timer;
        private bool running = false;

        public ActiveDebuff(IDebuff debuff, GameObject target)
        {
            this.debuff = debuff;
            this.target = target;
            this.duration = debuff.Duration;
            this.timer = duration;
            debuff.Apply(target);
        }

        public void ResetTimer()
        {
            timer = duration;
        }

        public IEnumerator Run(Action onEnd)
        {
            running = true;
            while (timer > 0)
            {
                timer -= Time.deltaTime;
                yield return null;
            }
            debuff.Remove(target);
            onEnd?.Invoke();
            running = false;
        }
    }

    private Dictionary<Type, ActiveDebuff> activeDebuffs = new Dictionary<Type, ActiveDebuff>();

    public void ApplyDebuff(IDebuff debuff)
    {
        Type debuffType = debuff.GetType();

        if (activeDebuffs.ContainsKey(debuffType))
        {
            activeDebuffs[debuffType].ResetTimer();
        }
        else
        {
            ActiveDebuff active = new ActiveDebuff(debuff, this.gameObject);
            activeDebuffs[debuffType] = active;
            StartCoroutine(active.Run(() => activeDebuffs.Remove(debuffType)));
        }
    }
}