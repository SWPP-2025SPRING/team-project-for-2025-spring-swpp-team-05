using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class SolveError : System.Exception
{
    public SolveError(string message) : base(message) { }
}

public class SolveCode
{
    private Queue<char> codeQueue = new Queue<char>();
    private float stunRate = 0f;
    private float stunDuration = 0f;
    private GameObject enemy;

    public SolveCode()
    {
        // Default constructor
    }

    public SolveCode(GameObject enemy, int codeLength, float rate, float duration = 0f)
    {
        string code = GenerateRandomCode(codeLength);
        Debug.Log("Generated Code: " + code);
        this.enemy = enemy;
        this.stunRate = rate;
        this.stunDuration = duration;

        foreach (char c in code)
        {
            codeQueue.Enqueue(c);
        }
    }

    public (GameObject, float, float) GetEnemy()
    {
        if (codeQueue.Count == 0)
        {
            return (enemy, stunRate, stunDuration);
        }
        else
        {
            throw new SolveError("Code is not solved yet");
        }
    }

    public float GetStunRate()
    {
        return stunRate;
    }

    public char GetNext()
    {
        if (codeQueue.Count > 0)
        {
            return codeQueue.Peek();
        }
        else
        {
            return ' ';
        }
    }

    public char GetNextNext()
    {
        if (codeQueue.Count > 1)
        {
            return this.PeekAt(1);
        }
        else
        {
            return ' ';
        }
    }

    public char GetNextNextNext()
    {
        if (codeQueue.Count > 2)
        {
            return this.PeekAt(2);
        }
        else
        {
            return ' ';
        }
    }

    public bool Solve(char input)
    {
        char expected = codeQueue.Peek();
        if (char.ToUpper(input) == char.ToUpper(expected))
        {
            codeQueue.Dequeue();
            if (codeQueue.Count == 0)
            {
                return true; // Code solved
            }
            return false; // Code not solved yet
        }
        else
        {
            throw new SolveError("Wrong Input : " + input + ", expected: " + expected);
        }
    }

    string GenerateRandomCode(int length)
    {
        const string chars = "BCEFGHIJKLMNOPQTUVXYZ4567890";
        System.Random rand = new System.Random();  // Unity Random은 MonoBehaviour밖에 사용 못함
        char[] result = new char[length];

        for (int i = 0; i < length; i++)
        {
            result[i] = chars[rand.Next(chars.Length)];
        }

        return new string(result);
    }

    private char PeekAt(int index)
    {
        char[] snapshot = codeQueue.ToArray();
        return index < snapshot.Length ? snapshot[index] : ' ';
    }
}