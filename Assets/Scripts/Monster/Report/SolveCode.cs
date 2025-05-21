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
            char next = codeQueue.Dequeue();
            char nextNext = codeQueue.Peek();
            codeQueue.Enqueue(next);
            return nextNext;
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
            char next = codeQueue.Dequeue();
            char nextNext = codeQueue.Dequeue();
            char nextNextNext = codeQueue.Peek();
            codeQueue.Enqueue(next);
            codeQueue.Enqueue(nextNext);
            return nextNextNext;
        }
        else
        {
            return ' ';
        }
    }

    public bool Solve(char input)
    {
        char expected = codeQueue.Peek();
        if (input == expected)
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
            throw new SolveError("Wrong Input");
        }
    }

    string GenerateRandomCode(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        System.Random rand = new System.Random();  // Unity Random은 MonoBehaviour밖에 사용 못함
        char[] result = new char[length];

        for (int i = 0; i < length; i++)
        {
            result[i] = chars[rand.Next(chars.Length)];
        }

        return new string(result);
    }
}