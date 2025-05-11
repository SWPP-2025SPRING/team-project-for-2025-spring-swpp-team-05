using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DebuffType
{
  SpeedDown,
  ReverseControl
}

public class Debuff
{
  public DebuffType Type;
  public float Duration;
  public float Timer;

  public Debuff(DebuffType type, float duration)
  {
    Type = type;
    Duration = duration;
    Timer = 0f;
  }
}
