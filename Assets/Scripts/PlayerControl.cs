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

    // Player Stats
    private float moveSpeed;
    private int attackPower;
    private float attackRange;

    private float defaultMoveSpeed = 10.0f;
    // private float maxMoveSpeed = 30.0f;
    private int defaultAttackPower = 10;
    private float defaultAttackRange = 3.0f;

    // Report Moster Interaction Variable
    public TextMeshProUGUI[] codeTexts;
    private float stunRateAgg = 0f;
    private Queue<SolveCode> stunQ = new Queue<SolveCode>();
    private char prevCode = ' ';
    private char currentCode = ' ';
    private char nextCode = ' ';
    private char nextNextCode = ' ';
    private bool isStun = false;


    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        Physics.gravity *= 1;
        initializePlayerStats();
        deactivateCodeText();
    }

    void initializePlayerStats()
    {
        moveSpeed = defaultMoveSpeed;
        attackPower = defaultAttackPower;
        attackRange = defaultAttackRange;

    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        // TODO: 킥보드로 바꾸고 나서는 킥보드 애니메이션으로 바꾸기

        if (Mathf.Abs(horizontalInput) > 0.01f)
        {
            Quaternion turnRotation = Quaternion.Euler(0, horizontalInput * 100 * Time.deltaTime, 0);
            transform.rotation *= turnRotation;
        }

        if (isStun)
        {
            string input = Input.inputString;
            if (input.Length > 0)
            {
                char inputChar = input[0];
                TrySolveStun(inputChar);
            }
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
        playerAnimator.SetFloat("Speed_f", moveSpeed / defaultMoveSpeed);
        Vector3 moveDirection = transform.forward;
        Vector3 newPosition = transform.position + moveDirection * moveSpeed * Time.deltaTime;
        playerRb.MovePosition(newPosition);

    }

    public void StunPlayer(SolveCode code)
    {
        if (!isStun)
        {
            prevCode = ' ';
            currentCode = code.GetNext();
            nextCode = code.GetNextNext();
            nextNextCode = code.GetNextNextNext();
            activateCodeText();
            UpdateCodeText();
        }
        isStun = true;
        stunRateAgg += code.GetStunRate();
        moveSpeed = defaultMoveSpeed * (1 - stunRateAgg);
        if (moveSpeed < 0)
        {
            moveSpeed = 0.01f;
        }
        stunQ.Enqueue(code);
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
                stunRateAgg -= rate;

                // 스피드 재계산
                if (stunRateAgg < 0)
                {
                    stunRateAgg = 0;
                }
                moveSpeed = defaultMoveSpeed * (1 - stunRateAgg);
                if (moveSpeed < 0)
                {
                    moveSpeed = 0.01f;
                }

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
                    isStun = false;
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
}