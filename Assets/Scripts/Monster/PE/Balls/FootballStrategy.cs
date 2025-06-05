using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class FootballStrategy : BallStrategy
{
    private BallType ballType = BallType.Football;
    private GameObject ballPrefab;
    private GameObject[] ballInstances;

    public override void OnBatched(Transform[] ballTransform)
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

            ballInstances[i] = Object.Instantiate(ballPrefab, ballTransform[i].position, ballTransform[i].rotation);
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

    public override IEnumerator OnAction(Animator animator, Transform[] ballTransform, Vector3[] force)
    {
        SetAnimation(animator);
        yield return new WaitForSeconds(0.5f);
        OnBatched(ballTransform);
        yield return new WaitForSeconds(0.5f);
        OnThrow(force);
        yield return new WaitForSeconds(0.5f);
    }
}