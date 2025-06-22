using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodeFactory
{
    private Queue<SolveCode> solveCodes = new Queue<SolveCode>();
    private char prevCode = ' ';
    private char currentCode = ' ';
    private char nextCode = ' ';
    private char nextNextCode = ' ';

    private void UpdateCodeText()
    {
        CodeUIManager.Instance.UpdateCodeUI(prevCode, currentCode, nextCode, nextNextCode);
    }

    public void AddCode(SolveCode code)
    {
        solveCodes.Enqueue(code);
        if (solveCodes.Count == 1)
        {
            // 첫 번째 코드가 추가되면 초기화
            prevCode = ' ';
            currentCode = code.GetNext();
            nextCode = code.GetNextNext();
            nextNextCode = code.GetNextNextNext();
            UpdateCodeText();
        }
    }

    public void TrySolveCode(char code)
    {
        try
        {
            SolveCode solveCode = solveCodes.Peek();
            bool isSolved = solveCode.Solve(code);
            if (isSolved)
            {
                // 이번 코드 해결
                (GameObject enemy, float rate, float duration) = solveCode.GetEnemy();
                PlayerStatus.instance.ReviveSlow(rate);
                enemy.GetComponent<ReportController>().EndMonster();

                solveCodes.Dequeue();

                if (solveCodes.Count > 0)
                {
                    SolveCode nextSolveCode = solveCodes.Peek();
                    prevCode = ' ';
                    currentCode = nextSolveCode.GetNext();
                    nextCode = nextSolveCode.GetNextNext();
                    nextNextCode = nextSolveCode.GetNextNextNext();
                    UpdateCodeText();
                }
                else
                {
                    // 더 이상 해결할 코드가 없음
                    prevCode = ' ';
                    currentCode = ' ';
                    nextCode = ' ';
                    nextNextCode = ' ';
                    CodeUIManager.Instance.DeactivateCodeUI();
                }
            }
            else
            {
                // 아직 코드 남아있음
                prevCode = currentCode;
                currentCode = nextCode;
                nextCode = nextNextCode;
                nextNextCode = solveCode.GetNextNextNext();
                UpdateCodeText();
            }
        }
        catch (SolveError e)
        {
            Debug.LogError($"Solve Error: {e.Message}");
        }
    }

    public void Reset()
    {
        solveCodes.Clear();
        prevCode = ' ';
        currentCode = ' ';
        nextCode = ' ';
        nextNextCode = ' ';
        PlayerStatus.instance.ResetSlow();
        UpdateCodeText();
        CodeUIManager.Instance.DeactivateCodeUI();
    }
}
