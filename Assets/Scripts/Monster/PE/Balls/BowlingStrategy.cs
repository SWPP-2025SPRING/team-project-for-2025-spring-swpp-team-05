using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public class BowlingStrategy : BallStrategy
{
    protected override BallType ballType => BallType.BowlingBall;
    private GameObject ballInstances;

    public override void OnBatched(Vector3[] ballTransform)
    {
        base.OnBatched(ballTransform);
        if (ballTransform == null || ballTransform.Length != 1)
        {
            Debug.LogError("Ball transform array is null or does not contain exactly one transform.");
            return;
        }
        ballInstances = UnityEngine.Object.Instantiate(ballPrefab, ballTransform[0], Quaternion.identity);
    }

    public override void OnThrow(Vector3[] force)
    {
        if (ballInstances == null)
        {
            Debug.LogError("Ball instance is null. Cannot apply force.");
            return;
        }

        if (force == null || force.Length != 1)
        {
            Debug.LogError("Force array is null or does not contain exactly one force vector.");
            return;
        }

        Rigidbody rb = ballInstances.GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody component not found on the ball instance.");
            return;
        }

        rb.AddForce(force[0], ForceMode.Force);
    }

    public override IEnumerator OnAction(Animator animator, Vector3[] ballTransform, Vector3[] force, Action onComplete = null)
    {
        SetAnimation(animator);
        yield return new WaitForSeconds(1.8f);
        OnBatched(ballTransform);
        OnThrow(force);
        yield return new WaitForSeconds(3f);
        onComplete?.Invoke();
    }
}