using UnityEngine;
using System;

public class ReverseControlDebuff : IDebuff
{
    public float Duration { get; private set; }

    public ReverseControlDebuff(float duration)
    {
        this.Duration = duration;
    }

    public void Apply(GameObject target)
    {
        PlayerStatus.instance.SetReverseControl(true);
    }

    public void Remove(GameObject target)
    {
        PlayerStatus.instance.SetReverseControl(false);
    }
}