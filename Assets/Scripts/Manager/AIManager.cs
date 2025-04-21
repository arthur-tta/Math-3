using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : Singleton<AIManager>
{
    private BaseAI playerA;
    private BaseAI playerB;
 

    public void SetUp(Mode modeA, Mode modeB)
    {
        switch (modeA)
        {
            case Mode.BruteForce:
                playerA = new BruteForce();
                break;
            case Mode.BruteForcePlus:
                break;
        }

        switch (modeB)
        {
            case Mode.BruteForce:
                playerB = new BruteForce();
                break;
            case Mode.BruteForcePlus:
                break;
        }
    }

    public Move GetPlayerANextMove(int[,] data)
    {
        return playerA.GetMove(data);
    }
    public Move GetPlayerBNextMove(int[,] data)
    {
        return playerB.GetMove(data);
    }
}
