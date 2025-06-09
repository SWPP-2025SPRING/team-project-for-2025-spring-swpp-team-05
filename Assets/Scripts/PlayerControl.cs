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
    private Vector3 moveDirection;


    // Report Moster Interaction Variable
    public TextMeshProUGUI[] codeTexts;
    private Queue<SolveCode> stunQ = new Queue<SolveCode>();
    private char prevCode = ' ';
    private char currentCode = ' ';
    private char nextCode = ' ';
    private char nextNextCode = ' ';

    // Ice Obstacle Interaction

    private bool isOnIce = false;
    private bool justEnteredIce = false;
    private Vector3 iceMomentum = Vector3.zero;
    private float iceBoostMultiplier = 3f;
    private float iceLerpFactor = 0.02f;


    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        Physics.gravity *= 1;
        deactivateCodeText();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        // TODO: 킥보드로 바꾸고 나서는 킥보드 애니메이션으로 바꾸기

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
                TrySolveStun(inputChar);
            }
        }


        // TODO: UI, player 테스트용, 후에 지우기 
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            PlayerStatus.instance.IncreaseSpeed();
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            PlayerStatus.instance.DecreaseSpeed();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            PlayerStatus.instance.IncreaseAttackPower();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            PlayerStatus.instance.IncreaseAttackRange();
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
        if (!PlayerStatus.instance.isSlow)
        {
            prevCode = ' ';
            currentCode = code.GetNext();
            nextCode = code.GetNextNext();
            nextNextCode = code.GetNextNextNext();
            activateCodeText();
            UpdateCodeText();
        }

        stunQ.Enqueue(code);
        PlayerStatus.instance.SlowPlayer(code.GetStunRate());
    }

    public void TrySolveStun(char code)
    {
        try
        {
            SolveCode solveCode = stunQ.Peek();
            bool isSolved = solveCode.Solve(code);
            if (isSolved)
            {
                // 이번 코드 해결
                (GameObject enemy, float rate, float duration) = solveCode.GetEnemy();
                PlayerStatus.instance.ReviveSlow(rate);
                // 몬스터 퇴치
                enemy.GetComponent<ReportController>().KnockOut();

                // 다음 코드로 넘어가기
                stunQ.Dequeue();

                // 다음 로직 결정
                if (stunQ.Count > 0)
                {
                    SolveCode nextSolveCode = stunQ.Peek();
                    prevCode = ' ';
                    currentCode = nextSolveCode.GetNext();
                    nextCode = nextSolveCode.GetNextNext();
                    nextNextCode = nextSolveCode.GetNextNextNext();
                }
                else
                {
                    // 더 이상 해결할 코드가 없음
                    prevCode = ' ';
                    currentCode = ' ';
                    nextCode = ' ';
                    nextNextCode = ' ';
                    deactivateCodeText();
                }
            }
            else
            {
                // 아직 코드 남아있음
                prevCode = currentCode;
                currentCode = nextCode;
                nextCode = nextNextCode;
                nextNextCode = solveCode.GetNextNextNext();
            }
            UpdateCodeText();
        }
        catch (SolveError e)
        {
            // 코드 풀기 실패
            Debug.Log("Code solving failed: " + e.Message);
        }
    }

    private void UpdateCodeText()
    {
        codeTexts[0].text = prevCode.ToString();
        codeTexts[1].text = currentCode.ToString();
        codeTexts[2].text = nextCode.ToString();
        codeTexts[3].text = nextNextCode.ToString();
    }

    private void activateCodeText()
    {
        foreach (TextMeshProUGUI codeText in codeTexts)
        {
            codeText.gameObject.SetActive(true);
        }
    }

    private void deactivateCodeText()
    {
        foreach (TextMeshProUGUI codeText in codeTexts)
        {
            codeText.gameObject.SetActive(false);
        }
    }

    public void EnterIceZone()
    {
        isOnIce = true;
        justEnteredIce = true;
        iceMomentum = Vector3.zero;
    }

    public void ExitIceZone()
    {
        isOnIce = false;
        justEnteredIce = false;
        iceMomentum = Vector3.zero;
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
}