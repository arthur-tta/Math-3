using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using Unity.Burst.Intrinsics;
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

    // combo & score
    public GameObject popupPrefab;
    private int combo;

    // setup game mode
    private bool isPlaying;
    private bool canPlay;

    private void Start()
    {
        SetUp();
        SetUpDiamondList();

        StartGame();
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

    public void StartGame()
    {
        StartCoroutine(InitializedBoard());
        canPlay = true;

    }

    private IEnumerator InitializedBoard()
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

        yield return new WaitForSeconds(0.5f);
        
        FindMatches();
    }

    

    public void CheckSwap(Move nextMove)
    {
        
        if (isPlaying == false || canPlay == false)
        {
            return;
        }

        int row = nextMove.position.x;
        int col = nextMove.position.y;
        Direction dir = nextMove.direction;
        switch (dir)
        {
            case Direction.Left:
                if (col > 0)
                {
                    SwapDiamond(nextMove);
                }
                break;

            case Direction.Right:
                if (col < columns - 1)
                {
                    SwapDiamond(nextMove);
                }

                break;
            case Direction.Top:
                if (row > 0)
                {
                    SwapDiamond(nextMove);
                }
                break;
            case Direction.Bottom:
                if (row < rows - 1)
                {
                    SwapDiamond(nextMove);
                }
                break;

        }
    }
    private void SwapDiamond(Move nextMove)
    {
        isPlaying = false;

        Vector2Int pos = nextMove.position;
        Vector2Int targetPos = pos;
        switch (nextMove.direction)
        {
            case Direction.Left: targetPos.y -= 1; break;
            case Direction.Right: targetPos.y += 1; break;
            case Direction.Top: targetPos.x -= 1; break;
            case Direction.Bottom: targetPos.x += 1; break;
        }

        // Hoán đổi giá trị trong mảng `data`
        int temp = data[pos.x, pos.y];
        data[pos.x, pos.y] = data[targetPos.x, targetPos.y];
        data[targetPos.x, targetPos.y] = temp;

        StartCoroutine(SwapWithAnimation(
            cells[pos.x * rows + pos.y].gameObject,
            cells[targetPos.x * rows + targetPos.y].gameObject
        ));

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

        yield return new WaitForSeconds(0.5f);
        FindMatches();
    }


    private void FindMatches()
    {
        HashSet<Vector2Int> matchSet = new HashSet<Vector2Int>();

        // Check hàng ngang
        for (int row = 0; row < rows; row++)
        {
            int matchCount = 1;
            for (int col = 1; col < columns; col++)
            {
                if (data[row, col] == data[row, col - 1] && data[row, col] != 0)
                {
                    matchCount++;
                }
                else
                {
                    if (matchCount >= 3)
                    {
                        for (int i = 0; i < matchCount; i++)
                        {
                            matchSet.Add(new Vector2Int(row, col - 1 - i));
                        }
                    }
                    matchCount = 1;
                }
            }
            if (matchCount >= 3)
            {
                for (int i = 0; i < matchCount; i++)
                {
                    matchSet.Add(new Vector2Int(row, columns - 1 - i));
                }
            }
        }

        // Check hàng dọc
        for (int col = 0; col < columns; col++)
        {
            int matchCount = 1;
            for (int row = 1; row < rows; row++)
            {
                if (data[row, col] == data[row - 1, col] && data[row, col] != 0)
                {
                    matchCount++;
                }
                else
                {
                    if (matchCount >= 3)
                    {
                        for (int i = 0; i < matchCount; i++)
                        {
                            matchSet.Add(new Vector2Int(row - 1 - i, col));
                        }
                    }
                    matchCount = 1;
                }
            }
            if (matchCount >= 3)
            {
                for (int i = 0; i < matchCount; i++)
                {
                    matchSet.Add(new Vector2Int(rows - 1 - i, col));
                }
            }
        }

        if (matchSet.Count > 0)
        {
            StartCoroutine(RemoveMatches(matchSet.ToList()));
            combo++;

            if(combo > 1)
            {
                GameObject comboPopup = Instantiate(popupPrefab, new Vector3(0, 100f, 0), Quaternion.identity);
                comboPopup.transform.SetParent(transform.parent, false);
                Popup popup = comboPopup.GetComponent<Popup>();
                popup.ShowPopup("Combo x" + combo);
                popup.GetComponent<TextMeshProUGUI>().color = Color.red;
            }
        }
        else
        {
            combo = 0;
            isPlaying = true;
        }
    }


    private IEnumerator RemoveMatches(List<Vector2Int> matches)
    {
        Debug.Log("remove mathches");

        yield return new WaitForSeconds(0.2f);
        foreach (var match in matches)
        {
            int row = match.x;
            int col = match.y;

            // Xóa dữ liệu trong mapData
            data[row, col] = 0;

            // Xóa vật thể trong game
            if (cells[row * rows + col].transform.childCount > 0)
            {
                Destroy(cells[row * rows + col].transform.GetChild(0).gameObject);
            }
        }

        DropAndFill();

        int score = CalculatorScore(matches);
        GameObject scorePopup = Instantiate(popupPrefab, Vector3.zero, Quaternion.identity);
        scorePopup.transform.SetParent(transform.parent, false);
        Popup popup = scorePopup.GetComponent<Popup>();
        popup.ShowPopup("+" + score);
        popup.GetComponent<TextMeshProUGUI>().color = Color.yellow;
    }


    private void DropAndFill()
    {
        for (int col = 0; col < columns; col++)
        {
            int emptyRow = -1; // Vị trí đầu tiên bị trống

            for (int row = rows - 1; row >= 0; row--)
            {
                if (data[row, col] == 0) // Nếu vị trí trống
                {
                    if (emptyRow == -1)
                    {
                        emptyRow = row;

                    }
                }
                else if (emptyRow != -1) // Nếu gặp vật thể phía trên
                {
                    // Cập nhật trong fruitData
                    if (data[emptyRow, col] == 0 && data[row, col] != 0)
                    {
                        data[emptyRow, col] = data[row, col];
                        data[row, col] = 0;

                        // Thêm animation rơi xuống nếu cần
                        StartCoroutine(DropAnimation(cells[row * rows + col].transform.GetChild(0).gameObject, cells[emptyRow * rows + col].gameObject));
                    }
                    emptyRow--; // Di chuyển xuống hàng tiếp theo
                }
            }

            // Tạo mới vật phẩm nếu còn khoảng trống
            for (int newRow = emptyRow; newRow >= 0; newRow--)
            {
                // Sinh vật phẩm mới trong game

                Diamond diamond = Instantiate(diamondList[Random.Range(0, diamondList.Count)], new Vector3(0, 1000f, 0), Quaternion.identity);
                data[newRow, col] = diamond.ID;
                //diamond.transform.SetParent(cells[newRow * rows + col].transform, false);
                //diamond.transform.localPosition = Vector3.zero;


                // Thêm animation nếu cần
                StartCoroutine(DropAnimation(diamond.gameObject, cells[newRow * rows + col].gameObject));

                diamond.transform.SetParent(cells[newRow * rows + col].transform, false);
            }
        }

        StartCoroutine(CheckMatchesAfterDrop());
    }
    private IEnumerator CheckMatchesAfterDrop()
    {
        yield return new WaitForSeconds(0.5f); // Đợi rơi xong
        FindMatches();
    }

    private IEnumerator DropAnimation(GameObject obj, GameObject newParent)
    {
        Vector3 startPosition = obj.transform.position;
        Vector3 endPosition = newParent.transform.position;

        float elapsedTime = 0f;
        float duration = 0.3f; // Thời gian rơi

        while (elapsedTime < duration)
        {
            obj.transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        obj.transform.SetParent(newParent.transform, false);
        obj.transform.localPosition = Vector3.zero;
    }

    private int CalculatorScore(List<Vector2Int> matches)
    {
        int score = 0;
        List<List<Vector2Int>> chains = FindValidChains(matches);

        for (int i = 0; i < chains.Count; i++)
        {
            switch (chains[i].Count)
            {
                case 3:
                    score += 30 * combo;
                    break;
                case 4:
                    score += 60 * combo;
                    break;
                default:
                    // danh cho nhung chuoi tu 5 tro len

                    //dang chu L hoac T
                    if (IsLShape(chains[i]))
                    {
                        score += 20 * chains[i].Count * combo;
                    }
                    // dang thang hang
                    else
                    {
                        score += 40 * chains[i].Count * combo;
                    }
                    break;
            }

        }
        return score;
    }

    private List<List<Vector2Int>> FindValidChains(List<Vector2Int> positions)
    {
        List<List<Vector2Int>> chains = new List<List<Vector2Int>>();
        HashSet<Vector2Int> visited = new HashSet<Vector2Int>();

        foreach (var pos in positions)
        {
            if (!visited.Contains(pos))
            {
                List<Vector2Int> chain = new List<Vector2Int>();
                DFS(pos, positions, visited, chain);
                if (chain.Count >= 3) // Chỉ lấy chuỗi có ít nhất 3 viên
                {
                    chains.Add(chain);
                }
            }
        }

        return chains;
    }

    private void DFS(Vector2Int current, List<Vector2Int> positions, HashSet<Vector2Int> visited, List<Vector2Int> chain)
    {
        visited.Add(current);
        chain.Add(current);

        // Các hướng di chuyển (trái, phải, trên, dưới)
        Vector2Int[] directions = { Vector2Int.left, Vector2Int.right, Vector2Int.up, Vector2Int.down };

        foreach (var dir in directions)
        {
            Vector2Int neighbor = current + dir;
            if (positions.Contains(neighbor) && !visited.Contains(neighbor))
            {
                DFS(neighbor, positions, visited, chain);
            }
        }
    }

    private bool IsLShape(List<Vector2Int> chain)
    {
        HashSet<Vector2Int> positions = new HashSet<Vector2Int>(chain);

        foreach (var pos in chain)
        {
            // Kiểm tra các trường hợp hình chữ L
            if (positions.Contains(pos + Vector2Int.right) &&
                positions.Contains(pos + Vector2Int.left) &&
                positions.Contains(pos + Vector2Int.up))
            {
                return true;
            }

            if (positions.Contains(pos + Vector2Int.up) &&
                positions.Contains(pos + Vector2Int.down) &&
                positions.Contains(pos + Vector2Int.right))
            {
                return true;
            }
        }

        return false;
    }








    private void SetUpDiamondList()
    {
        for (int i = 0; i < diamondTotal; i++)
        {
            if (diamonds.Count > 0)
            {
                int k = Random.Range(0, diamonds.Count);
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
}
