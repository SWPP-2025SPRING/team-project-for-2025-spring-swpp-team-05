using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public abstract class BallStrategy : IBallStrategy
{
    protected abstract BallType ballType { get; }
    protected GameObject ballPrefab;
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

    public virtual void OnBatched(Vector3[] ballTransform)
    {
        ballPrefab = BallManager.Instance.GetBallPrefab(ballType);
    }
    public abstract void OnThrow(Vector3[] force);
    public abstract IEnumerator OnAction(Animator animator, Vector3[] ballTransform, Vector3[] force);
}