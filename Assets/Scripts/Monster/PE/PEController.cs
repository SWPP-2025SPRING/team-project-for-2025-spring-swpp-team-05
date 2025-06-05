using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PEController : MonoBehaviour
{
    private GameObject player;
    private Animator animator;

    public float force = 100f;
    public float summonInterval = 10f;
    private float cooldownTimer = 0f;

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
            summonBall();
        }
        else if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
        }
    }

    void summonBall()
    {
        if (player == null)
        {
            Debug.LogError("Player object is not set. Cannot summon ball.");
            return;
        }

        Vector3[] ballTransforms = new Vector3[7];
        Vector3[] forces = new Vector3[7];
        for (int i = 0; i < ballTransforms.Length; i++)
        {
            // Assuming you want to position the balls around the player
            ballTransforms[i] = transform.position + new Vector3(i * 0.5f, 0, 0); // Adjust the position as needed

            Vector3 direction = (player.transform.position - ballTransforms[i]).normalized + Vector3.up * 0.1f;
            forces[i] = direction * force; // Apply the force in the direction away from the player
        }

        IBallStrategy ballStrategy = new FootballStrategy(); // Replace with the desired ball strategy
        StartCoroutine(ballStrategy.OnAction(animator, ballTransforms, forces));
        Debug.Log("Summoned balls with force: " + force);
    }
}
