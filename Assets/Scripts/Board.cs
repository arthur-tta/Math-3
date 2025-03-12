using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : Singleton<Board>
{
    private int rows = 9, columns = 9;
    // data
    private int[,] data;

    // cell
    public Cell cellPrefab;
    private List<Cell> cells;

    // diamon
    public int diamondTotal;
    public List<Diamond> diamonds = new List<Diamond>();
    private List<Diamond> diamondList = new List<Diamond>();

    private void Start()
    {
        SetUp();
        SetUpDiamondList();
        InitializedBoard();
    }

    private void SetUp()
    {
        data = new int[rows, columns];
        cells = new List<Cell>();

        Cell cell;
        for(int row = 0; row < rows; row++)
        {
            for(int col = 0; col < columns; col++)
            {
                cell = Instantiate(cellPrefab, Vector3.zero, Quaternion.identity);
                cells.Add(cell);
                cell.transform.SetParent(transform, false);
                cell.SetUp(new Vector2Int(row, col));
            }
        }
    }

    public void InitializedBoard()
    {
        Diamond diamond;
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                diamond = Instantiate(diamondList[Random.Range(0, diamondList.Count)], Vector3.zero, Quaternion.identity);
                data[row, col] = diamond.ID;
                diamond.transform.SetParent(cells[row * rows + col].transform, false);
            }
        }
    }

    private void SetUpDiamondList()
    {
        for(int i = 0; i < diamondTotal; i++)
        {
            if(diamonds.Count > 0)
            {
                int k  = Random.Range(0, diamonds.Count);
                diamondList.Add(diamonds[k]);
                diamonds.RemoveAt(k);
            }
            else
            {
                diamondList.Add(diamonds[0]);
                diamonds.RemoveAt(0);
            }
        }
    }

    public void CheckSwap(Move nextMove)
    {
        
        switch(nextMove.direction) 
        {
            case Direction.Left:
                if(nextMove.position.x > 0)
                {
                    SwapDiamond(nextMove);
                }
                break;
            case Direction.Right:
                if (nextMove.position.x < columns - 1)
                {
                    SwapDiamond(nextMove);
                }
                break;
            case Direction.Top:
                if (nextMove.position.y > 0)
                {
                    SwapDiamond(nextMove);
                }
                break;
            case Direction.Bottom:
                if (nextMove.position.y < rows - 1)
                {
                    SwapDiamond(nextMove);
                }
                break;
        }
    }

    private void SwapDiamond(Move nextMove)
    {
        Debug.Log(nextMove.direction);
        Vector2Int pos = nextMove.position;
        switch (nextMove.direction)
        {
            case Direction.Left:
                StartCoroutine(SwapWithAnimation(cells[pos.x * rows + pos.y].gameObject, cells[pos.x * rows + pos.y - 1].gameObject));
                break;
            case Direction.Right:
                StartCoroutine(SwapWithAnimation(cells[pos.x * rows + pos.y].gameObject, cells[pos.x * rows + pos.y + 1].gameObject));
                break;
            case Direction.Top:
                StartCoroutine(SwapWithAnimation(cells[pos.x * rows + pos.y].gameObject, cells[(pos.x - 1) * rows + pos.y].gameObject));
                break;
            case Direction.Bottom:
                StartCoroutine(SwapWithAnimation(cells[pos.x * rows + pos.y].gameObject, cells[(pos.x + 1) * rows + pos.y].gameObject));
                break;
        }
        
    }

    private IEnumerator SwapWithAnimation(GameObject cell1, GameObject cell2)
    {
        GameObject diamond1 = cell1.transform.GetChild(0).gameObject;
        GameObject diamond2 = cell2.transform.GetChild(0).gameObject;

        Vector3 startPos = diamond1.transform.position;
        Vector3 targetPos = diamond2.transform.position;

        float duration = 0.2f;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            diamond1.transform.position = Vector3.Lerp(startPos, targetPos, elapsedTime / duration);
            diamond2.transform.position = Vector3.Lerp(targetPos, startPos, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        diamond1.transform.SetParent(cell2.transform, false);
        diamond1.transform.localPosition = Vector3.zero;

        diamond2.transform.SetParent(cell1.transform, false);
        diamond2.transform.localPosition = Vector3.zero;
    }
}
