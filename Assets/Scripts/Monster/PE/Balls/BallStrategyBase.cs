using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public abstract class BallStrategy : IBallStrategy
{
    private readonly BallType ballType = BallType.None;
    private GameObject ballPrefab;
    public virtual void SetAnimation(Animator animator)
    {
        if (animator == null)
        {
            Debug.LogError("Animator is null. Cannot set animation.");
            return;
        }

        string trigger = BallManager.Instance.GetBallTrigger(ballType);
        if (string.IsNullOrEmpty(trigger))
        {
            Debug.LogError($"No trigger found for ball type {ballType}.");
            return;
        }

        animator.SetTrigger(trigger);
    }

    public virtual void OnBatched(Transform[] ballTransform)
    {
        ballPrefab = BallManager.Instance.GetBallPrefab(ballType);
    }
    public abstract void OnThrow(Vector3[] force);
    public abstract IEnumerator OnAction(Animator animator, Transform[] ballTransform, Vector3[] force);
}