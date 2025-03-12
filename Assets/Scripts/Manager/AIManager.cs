using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : Singleton<AIManager>
{
    private BaseAI currentAI;
    private Difficulty difficulty;

    public void StartGame(Difficulty difficulty)
    {
        this.difficulty = difficulty;
        switch (difficulty)
        {
            case Difficulty.BruteForce:
                //this.currentAI;
                break;
        }
    }

    public Move GetNextMove(int[,] data)
    {
        return currentAI.GetMove(data);
    }

    public Difficulty GetDifficulty()
    {
        return difficulty;
    }
}
