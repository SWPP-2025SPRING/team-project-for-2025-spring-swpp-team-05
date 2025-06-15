using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

enum AttackType
{
    FootballConverge,
    FootballDiverge,
    BowlingBall,
    Basketball,
    BeachBall,
    TennisBall
}

public class PEController : MonoBehaviour, IMonsterController
{
    private GameObject player;
    private Animator animator;

    [Header("Attack Force Settings")]
    public float footballForce = 10f;
    public float bowlingForce = 20000f;
    public float basketballForce = 10f;
    public float beachBallForce = 7f;
    public float tennisBallForce = 5f;

    public float summonInterval = 100f;
    public float maxRotationSpeed = 10f;
    private float cooldownTimer = 0f;
    private bool isAttacking = false;
    private Action onComplete;

    public void SetLevel(int level)
    {
        // --- Growth Rates ---
        float footballForceGrowth = 0.05f;
        float bowlingForceGrowth = 0.1f;
        float basketballForceGrowth = 0.05f;
        float beachBallForceGrowth = 0.03f;
        float tennisBallForceGrowth = 0.02f;

        float summonIntervalGrowth = 0.05f; // Adjust the growth rate for summon interval

        // --- Apply Growth ---
        footballForce += footballForce * footballForceGrowth * level;
        bowlingForce += bowlingForce * bowlingForceGrowth * level;
        basketballForce += basketballForce * basketballForceGrowth * level;
        beachBallForce += beachBallForce * beachBallForceGrowth * level;
        tennisBallForce += tennisBallForce * tennisBallForceGrowth * level;
        summonInterval -= summonInterval * summonIntervalGrowth * level; // Decrease interval as level increases
    }
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component not found on the PEController GameObject.");
            return;
        }
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player object not found! Make sure the player has the 'Player' tag.");
        }
        else
        {
            Debug.Log("Player object found: " + player.name);
        }
        onComplete = () =>
        {
            isAttacking = false;
            Debug.Log("Attack completed .");
        };
    }

    // Update is called once per frame
    void Update()
    {
        if (cooldownTimer <= 0f)
        {
            cooldownTimer = summonInterval;
            isAttacking = true;
            OnAttack(); // Summon a tennis ball as the default attack
        }

        if (!isAttacking)
        {
            RotateTowardsPlayer();
        }

        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
        }
    }

    void RotateTowardsPlayer()
    {
        if (player == null)
        {
            Debug.LogError("Player object is not set. Cannot rotate towards player.");
            return;
        }

        Vector3 direction = (player.transform.position - transform.position).normalized;
        direction.y = 0; // Keep the rotation on the horizontal plane
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        Vector3 forward = transform.forward;
        float angle = Vector3.SignedAngle(forward, direction, Vector3.up);
        Debug.Log("Angle to player: " + angle);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, maxRotationSpeed * Time.deltaTime);
        //animator.SetFloat("turnSpeed_f", angle / 180f);
    }

    public void OnAttack()
    {
        int count;
        AttackType attackType = (AttackType)Random.Range(0, System.Enum.GetValues(typeof(AttackType)).Length);
        switch (attackType)
        {
            case AttackType.FootballConverge:
                count = Random.Range(1, 10); // Randomize the number of footballs
                SummonFootBallConverge(count);
                break;
            case AttackType.FootballDiverge:
                count = Random.Range(3, 10); // Randomize the number of footballs
                SummonFootBallDiverge(count);
                break;
            case AttackType.BowlingBall:
                SummonBowlingBall();
                break;
            case AttackType.Basketball:
                SummonBasketBall();
                break;
            case AttackType.BeachBall:
                SummonBeachBall();
                break;
            case AttackType.TennisBall:
                SummonTennisBall();
                break;
            default:
                Debug.LogWarning("Unknown attack type summoned: " + attackType);
                break;
        }
    }

    void SummonFootBallConverge(int count = 7)
    {
        Vector3[] ballTransforms = new Vector3[count];
        Vector3[] forces = new Vector3[count];
        for (int i = 0; i < ballTransforms.Length; i++)
        {
            // Assuming you want to position the balls around the player
            ballTransforms[i] = transform.position + transform.forward * 2f + ((i - count / 2) * transform.right);

            Vector3 direction = (player.transform.position - ballTransforms[i]).normalized + Vector3.up * 0.5f;
            forces[i] = direction * footballForce; // Apply the force in the direction away from the player
        }

        IBallStrategy ballStrategy = new FootballStrategy(); // Replace with the desired ball strategy
        StartCoroutine(ballStrategy.OnAction(animator, ballTransforms, forces, onComplete));
        Debug.Log("Summoned balls with force: " + footballForce);
    }

    void SummonFootBallDiverge(int count = 7)
    {
        Vector3 origin = transform.position + transform.forward * 2f;
        Vector3[] ballPositions = new Vector3[count];
        Vector3[] forces = new Vector3[count];

        float angleStep = 360f / count;
        float radius = 0.5f;

        for (int i = 0; i < count; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            Vector3 offset = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;

            ballPositions[i] = origin + offset;

            Vector3 dir = (offset).normalized + Vector3.up * 0.3f;
            forces[i] = dir * footballForce;
        }

        IBallStrategy ballStrategy = new FootballStrategy();
        StartCoroutine(ballStrategy.OnAction(animator, ballPositions, forces, onComplete));
        Debug.Log("Diverged balls with force: " + footballForce);
    }

    void SummonBowlingBall()
    {
        Vector3[] ballTransforms = new Vector3[1];
        Vector3[] forces = new Vector3[1];

        ballTransforms[0] = transform.position + transform.forward * 2f;
        Vector3 direction = (player.transform.position - ballTransforms[0]).normalized;
        forces[0] = direction * bowlingForce;

        IBallStrategy ballStrategy = new BowlingStrategy(); // Replace with the desired ball strategy
        StartCoroutine(ballStrategy.OnAction(animator, ballTransforms, forces, onComplete));
        Debug.Log("Summoned bowling ball with force: " + bowlingForce);
    }

    void SummonBasketBall()
    {
        Vector3[] ballTransforms = new Vector3[1];
        Vector3[] forces = new Vector3[1];

        ballTransforms[0] = transform.position + transform.forward * 2f + Vector3.up * 3f;
        Vector3 direction = (player.transform.position - ballTransforms[0]).normalized; // Add some upward force to simulate a basketball throw
        forces[0] = direction * basketballForce;

        IBallStrategy ballStrategy = new SingleBallStrategy(BallType.Basketball, ForceMode.Impulse, 0.5f, 1f, 1f); // Replace with the desired ball strategy
        StartCoroutine(ballStrategy.OnAction(animator, ballTransforms, forces, onComplete));
        Debug.Log("Summoned basketball with force: " + footballForce);
    }

    void SummonBeachBall()
    {
        Vector3[] ballTransforms = new Vector3[1];
        Vector3[] forces = new Vector3[1];

        ballTransforms[0] = transform.position + transform.forward * 2f + Vector3.up * 3f;
        Vector3 direction = (player.transform.position - ballTransforms[0]).normalized; // Add some upward force to simulate a basketball throw
        forces[0] = direction * beachBallForce;

        IBallStrategy ballStrategy = new SingleBallStrategy(BallType.BeachBall, ForceMode.Impulse, 0.5f, 1f, 1f); // Replace with the desired ball strategy
        StartCoroutine(ballStrategy.OnAction(animator, ballTransforms, forces, onComplete));
        Debug.Log("Summoned beachball with force: " + footballForce);
    }

    void SummonTennisBall()
    {
        Vector3[] ballTransforms = new Vector3[1];
        Vector3[] forces = new Vector3[1];

        ballTransforms[0] = transform.position + transform.forward * 2f + Vector3.up * 3f;
        Vector3 direction = (player.transform.position - ballTransforms[0]).normalized; // Add some upward force to simulate a basketball throw
        forces[0] = direction * tennisBallForce;

        IBallStrategy ballStrategy = new SingleBallStrategy(BallType.TennisBall, ForceMode.Impulse, 2.8f, 0f, 2f); // Replace with the desired ball strategy
        StartCoroutine(ballStrategy.OnAction(animator, ballTransforms, forces, onComplete));
        Debug.Log("Summoned tennisball with force: " + footballForce);
    }

    public void EndMonster()
    {
        // Implement any cleanup or end logic for the monster here
        Debug.Log("PEController EndMonster called.");
        Destroy(gameObject); // Example: destroy the monster GameObject
    }
}
