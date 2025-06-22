using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
    private float verticalInput;
    private Vector3 moveDirection;

    // Code Interaction
    private CodeFactory codeFactory;

    // Ice Obstacle Interaction

    private bool isOnIce = false;
    private bool justEnteredIce = false;
    private Vector3 iceMomentum = Vector3.zero;
    private float iceBoostMultiplier = 3f;
    private float iceLerpFactor = 0.02f;

    // Acceleration 
    private float currentSpeed = 0f;
    private bool isBraking = false;


    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        Physics.gravity *= 1;

        // Initialize Code Factory
        codeFactory = new CodeFactory();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        // TODO: 킥보드로 바꾸고 나서는 킥보드 애니메이션으로 바꾸기
        if (PlayerStatus.instance.isReverseControl)
        {
            horizontalInput *= -1f;
        }
        if (!isOnIce && Mathf.Abs(horizontalInput) > 0.01f)
        {
            Quaternion turnRotation = Quaternion.Euler(0, horizontalInput * 100 * Time.deltaTime, 0);
            transform.rotation *= turnRotation;
        }

        if (PlayerStatus.instance.isSlow)
        {
            string input = Input.inputString;
            if (input.Length > 0)
            {
                char inputChar = input[0];
                codeFactory.TrySolveCode(inputChar);
            }
        }

        if (horizontalInput == 1f)
        {
            playerAnimator.SetBool("TurnRight", true);
            playerAnimator.SetBool("TurnLeft", false);
        }
        else if (horizontalInput == -1f)
        {
            playerAnimator.SetBool("TurnLeft", true);
            playerAnimator.SetBool("TurnRight", false);
        }
        else
        {
            playerAnimator.SetBool("TurnLeft", false);
            playerAnimator.SetBool("TurnRight", false);
        }

        if (verticalInput < 0f && !isBraking)
        {
            isBraking = true;
        }
        else if (verticalInput >= 0f && isBraking)
        {
            isBraking = false;
        }


    }

    void FixedUpdate()
    {
        if (gameManager.isGameActive)
        {
            MovePlayerForward();
            if (isBraking)
            {
                float decelerationFactor = isOnIce ? 0.5f : 1.0f;
                PlayerStatus.instance.DeAccelerate(decelerationFactor, Time.deltaTime);
            }
            else
            {
                float accelerationFactor = isOnIce ? 2.0f : 1.0f;
                PlayerStatus.instance.Accelerate(accelerationFactor, Time.deltaTime);
            }

        }
    }

    void MovePlayerForward()
    {
        Vector3 velocity;
        if (isOnIce)
        {
            velocity = HandleIceMovement(transform.forward);
            playerAnimator.speed = 0.2f;
        }
        else
        {
            velocity = transform.forward * PlayerStatus.instance.moveSpeed;
            playerAnimator.speed = 1.0f;
        }

        Vector3 newPosition = transform.position + velocity * Time.deltaTime;
        playerRb.MovePosition(newPosition);

        // 애니메이션 동작 결정
        float animationSpeed = PlayerStatus.instance.moveSpeed / PlayerStatus.instance.defaultMoveSpeed;
        playerAnimator.SetFloat("Speed_f", animationSpeed);
    }

    public void StunPlayer(SolveCode code)
    {
        codeFactory.AddCode(code);
        PlayerStatus.instance.SlowPlayer(code.GetStunRate());
    }
    public void EnterIceZone()
    {
        isOnIce = true;
        justEnteredIce = true;
        iceMomentum = Vector3.zero;
        DebufManager.Instance.UpdateDebufText(DebufType.Slip);
    }

    public void ExitIceZone()
    {
        isOnIce = false;
        justEnteredIce = false;
        iceMomentum = Vector3.zero;
        DebufManager.Instance.UpdateDebufText(DebufType.None);
    }

    private Vector3 HandleIceMovement(Vector3 baseDirection)
    {
        if (justEnteredIce)
        {
            iceMomentum = baseDirection * PlayerStatus.instance.moveSpeed * iceBoostMultiplier;
            justEnteredIce = false;
        }
        else
        {
            Vector3 target = baseDirection * PlayerStatus.instance.moveSpeed;
            iceMomentum = Vector3.Lerp(iceMomentum, target, iceLerpFactor);
        }

        return iceMomentum;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            StartCoroutine(TriggerAttacked());
        }
    }

    private IEnumerator TriggerAttacked()
    {
        playerAnimator.SetBool("Attacked", true);
        yield return null; // 다음 프레임까지 대기
        playerAnimator.SetBool("Attacked", false);
    }
}
