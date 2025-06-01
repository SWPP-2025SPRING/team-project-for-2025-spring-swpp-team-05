using UnityEngine;

public class SpeedDebuff : IDebuff
{
    private float slowRate;
    public float Duration { get; private set; }

    public SpeedDebuff(float slowRate, float duration)
    {
        this.slowRate = slowRate;
        this.Duration = duration;
    }

    public void Apply(GameObject target)
    {
        PlayerStatus.instance.SlowPlayer(slowRate);
    }

    public void Remove(GameObject target)
    {
        PlayerStatus.instance.ReviveSlow(slowRate);
    }
}