using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

enum AttackType
{
    FootballConverge,
    FootballDiverge,
    BowlingBall,
}

public class PEController : MonoBehaviour
{
    private GameObject player;
    private Animator animator;

    public float footballForce = 10f;
    public float bowlingForce = 20000f;
    public float summonInterval = 100f;
    public float maxRotationSpeed = 10f;
    private float cooldownTimer = 0f;
    private bool isAttacking = false;

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
    }

    // Update is called once per frame
    void Update()
    {
        if (cooldownTimer <= 0f)
        {
            cooldownTimer = summonInterval;
            SummonRandom();
        }
        else if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
            RotateTowardsPlayer();
        }
        else
        {
            RotateTowardsPlayer();
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
        //transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, maxRotationSpeed * Time.deltaTime);
        animator.SetFloat("turnSpeed_f", angle / 180f);
    }

    void SummonRandom()
    {
        int count = Random.Range(1, 10);
        AttackType attackType = (AttackType)Random.Range(0, System.Enum.GetValues(typeof(AttackType)).Length);
        switch (attackType)
        {
            case AttackType.FootballConverge:
                SummonFootBallConverge(count);
                break;
            case AttackType.FootballDiverge:
                SummonFootBallDiverge(count);
                break;
            case AttackType.BowlingBall:
                SummonBowlingBall();
                break;
            default:
                Debug.LogWarning("Unknown attack type summoned: " + attackType);
                break;
        }
    }

    void SummonFootBallConverge(int count = 7)
    {
        if (player == null)
        {
            Debug.LogError("Player object is not set. Cannot summon ball.");
            return;
        }

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
        StartCoroutine(ballStrategy.OnAction(animator, ballTransforms, forces));
        Debug.Log("Summoned balls with force: " + footballForce);
    }

    void SummonFootBallDiverge(int count = 7)
    {
        if (player == null)
        {
            Debug.LogError("Player object is not set. Cannot summon ball.");
            return;
        }

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
        StartCoroutine(ballStrategy.OnAction(animator, ballPositions, forces));
        Debug.Log("Diverged balls with force: " + footballForce);
    }

    void SummonBowlingBall()
    {
        if (player == null)
        {
            Debug.LogError("Player object is not set. Cannot summon ball.");
            return;
        }

        Vector3[] ballTransforms = new Vector3[1];
        Vector3[] forces = new Vector3[1];

        ballTransforms[0] = transform.position + transform.forward * 2f;
        Vector3 direction = (player.transform.position - ballTransforms[0]).normalized;
        forces[0] = direction * bowlingForce;

        IBallStrategy ballStrategy = new BowlingStrategy(); // Replace with the desired ball strategy
        StartCoroutine(ballStrategy.OnAction(animator, ballTransforms, forces));
        Debug.Log("Summoned bowling ball with force: " + bowlingForce);
    }

}
