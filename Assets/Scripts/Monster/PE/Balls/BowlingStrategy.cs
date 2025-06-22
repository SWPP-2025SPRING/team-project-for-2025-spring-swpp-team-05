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

    public override IEnumerator OnTrackAction(Animator animator, Vector3[] ballTransform, GameObject player, float force, Action onComplete = null)
    {
        SetAnimation(animator);
        yield return new WaitForSeconds(1.8f);
        OnBatched(ballTransform);
        Vector3 playerPosition = player.transform.position;
        Vector3[] forceVector = new Vector3[ballTransform.Length];
        for (int i = 0; i < ballTransform.Length; i++)
        {
            Vector3 direction = (playerPosition - ballTransform[i]).normalized;
            forceVector[i] = direction * force;
        }
        OnThrow(forceVector);
        yield return new WaitForSeconds(3f);
        onComplete?.Invoke();
    }
}