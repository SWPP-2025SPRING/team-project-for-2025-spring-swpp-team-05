using UnityEngine;
using System;

public interface IDebuff
{
    void Apply(GameObject target);
    void Remove(GameObject target);
    float Duration { get; }
}