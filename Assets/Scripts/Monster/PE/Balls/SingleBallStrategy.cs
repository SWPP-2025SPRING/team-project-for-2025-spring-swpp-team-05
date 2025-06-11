using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using Object = UnityEngine.Object;

public class SingleBallStrategy : BallStrategy
{
    private readonly BallType _ballType;
    protected override BallType ballType => _ballType;
    private GameObject ballInstances;
    private ForceMode forceMode;
    protected float batchDelay;
    protected float throwDelay;
    protected float endDelay;

    public SingleBallStrategy(
        BallType ballType,
        ForceMode forceMode = ForceMode.Impulse,
        float batchDelay = 1.8f,
        float throwDelay = 3f,
        float endDelay = 1.8f)
    {
        _ballType = ballType;
        this.forceMode = forceMode;
        this.batchDelay = batchDelay;
        this.throwDelay = throwDelay;
        this.endDelay = endDelay;
    }

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

        rb.AddForce(force[0], forceMode);
    }

    public override IEnumerator OnAction(Animator animator, Vector3[] ballTransform, Vector3[] force, Action onComplete = null)
    {
        SetAnimation(animator);
        yield return new WaitForSeconds(batchDelay);
        OnBatched(ballTransform);
        yield return new WaitForSeconds(throwDelay);
        OnThrow(force);
        yield return new WaitForSeconds(endDelay);
        onComplete?.Invoke();
    }
}