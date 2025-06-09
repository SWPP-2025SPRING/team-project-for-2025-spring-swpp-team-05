using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using Object = UnityEngine.Object;

public class FootballStrategy : BallStrategy
{
    protected override BallType ballType => BallType.Football;
    private GameObject[] ballInstances;

    public override void OnBatched(Vector3[] ballTransform)
    {
        base.OnBatched(ballTransform);
        ballInstances = new GameObject[ballTransform.Length];
        for (int i = 0; i < ballTransform.Length; i++)
        {
            if (ballTransform[i] == null)
            {
                Debug.LogError($"Ball transform at index {i} is null.");
                continue;
            }

            ballInstances[i] = Object.Instantiate(ballPrefab, ballTransform[i], Quaternion.identity);
        }
    }

    public override void OnThrow(Vector3[] force)
    {
        if (ballInstances.Length != force.Length)
        {
            Debug.LogError($"Number of ball instances ({ballInstances.Length}) does not match the number of forces ({force.Length}).");
            return;
        }
        for (int i = 0; i < ballInstances.Length; i++)
        {
            if (ballInstances[i] == null)
            {
                Debug.LogError($"Ball instance at index {i} is null.");
                continue;
            }

            Rigidbody rb = ballInstances[i].GetComponent<Rigidbody>();
            if (rb == null)
            {
                Debug.LogError($"Rigidbody component not found on ball instance at index {i}.");
                continue;
            }

            rb.AddForce(force[i], ForceMode.Impulse);
        }
    }

    public override IEnumerator OnAction(Animator animator, Vector3[] ballTransform, Vector3[] force, Action onComplete = null)
    {
        SetAnimation(animator);
        yield return new WaitForSeconds(2f);
        OnBatched(ballTransform);
        yield return new WaitForSeconds(4f);
        OnThrow(force);
        yield return new WaitForSeconds(3f);
        onComplete?.Invoke();
    }
}