using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private Difficultly difficultly;


    private void Start()
    {
        UIManager.Instance.OpenUI<UIMainMenu>();
    }

    public void StartTestGame(Mode modeA, Mode modeB)
    { 
        AIManager.Instance.SetUp(modeA, modeB);
    }
    
    public void StartTrainGame()
    {

    }
}
