using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{

    // Inspector: connect
    public GameManager gameManager;

    // Basic
    private Rigidbody playerRb;
    private Animator playerAnimator;
    private bool isGround;

    // Audio
    private AudioSource playerAudio;

    // Move Action
    // private float forwardInput;
    private float horizontalInput;
    private float moveSpeed = 10.0f;
    private Vector3 moveDirection;

    // Player Stats
    // private int playerHP;
    // private int attackPower;
    // private float attackRange;

    // private int defaultHP = 100;
    // private int maxHP = 100;
    // private int defaultAttackPower = 10;
    // private float defaultAttackRange = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        Physics.gravity *= 1;
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        playerAnimator.SetFloat("Speed_f", 0.6f);

        if (Mathf.Abs(horizontalInput) > 0.01f)
        {
            Quaternion turnRotation = Quaternion.Euler(0, horizontalInput * 100 * Time.deltaTime, 0);
            transform.rotation *= turnRotation;
        }
    }

    void FixedUpdate()
    {
        if (gameManager.isGameActive)
        {
            MovePlayerForward();
        }
    }

    void MovePlayerForward()
    {
        Vector3 moveDirection = transform.forward;
        Vector3 newPosition = transform.position + moveDirection * moveSpeed * Time.deltaTime;
        playerRb.MovePosition(newPosition);

    }

}
