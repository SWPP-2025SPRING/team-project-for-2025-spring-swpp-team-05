using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public interface IBallStrategy
{
    void SetAnimation(Animator animator);
    void OnBatched(Vector3[] ballTransform);
    void OnThrow(Vector3[] force);
    IEnumerator OnAction(Animator animator, Vector3[] ballTransform, Vector3[] force, Action onComplete);
}