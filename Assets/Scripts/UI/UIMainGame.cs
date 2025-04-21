using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIMainGame : UICanvas
{
    // Target
    public TextMeshProUGUI targetTxt;
    private int target;

    // Player A
    public TextMeshProUGUI playerAScoreTxt;
    private int playerAScore;
    public GameObject playerATurn;

    //Player B
    public TextMeshProUGUI playerBScoreTxt;
    private int playerBScore;
    public GameObject playerBTurn;

    // Turn
    private Turn turn;

    public void Initialized(int target)
    {
        target = target;
        targetTxt.text = target.ToString();

        turn = Turn.PlayerA;

        playerAScore = 0;
        playerBScore = 0;
        playerAScoreTxt.text = playerAScore.ToString();
        playerBScoreTxt.text = playerBScore.ToString();

        playerATurn.gameObject.SetActive(true);
        playerBTurn.gameObject.SetActive(false);
    }

    public void NextTurn()
    {
        CheckWin();
        if (turn == Turn.PlayerA)
        {
            turn = Turn.PlayerB;

            playerATurn.gameObject.SetActive(false);
            playerBTurn.gameObject.SetActive(true);
        }
        else
        {
            turn = Turn.PlayerA;

            playerATurn.gameObject.SetActive(true);
            playerBTurn.gameObject.SetActive(false);

        }
    }

    public void IncreaseScore(int score)
    {
        if (turn == Turn.PlayerA)
        {
            StartCoroutine(AnimateText(playerAScoreTxt, playerAScore, playerAScore + score));
            playerAScore += score;

        }
        else
        {
            StartCoroutine(AnimateText(playerBScoreTxt, playerBScore, playerBScore + score));
            playerBScore += score;
        }
    }

    private IEnumerator AnimateText(TextMeshProUGUI text, int startValue, int endValue)
    {
        float duration = 1f;
        float time = 0;
        while (time < duration)
        {
            int currentValue = Mathf.RoundToInt(Mathf.Lerp(startValue, endValue, time / duration));
            text.text = currentValue.ToString();
            time += Time.deltaTime;
            yield return null;
        }

        // Đảm bảo giá trị cuối cùng chính xác
        text.text = endValue.ToString();
    }

    private void CheckWin()
    {
        if (playerAScore >= 10000 || playerBScore >= 10000)
        {
            Board.Instance.ResetGame();
        }
    }
}
