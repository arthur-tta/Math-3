using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class BaseAI : MonoBehaviour
{
    public abstract Move GetMove(int[,] data);

    public List<Direction> directions = new List<Direction>()
    {
        Direction.Left,
        Direction.Right,
        Direction.Top,
        Direction.Bottom,
    };

    protected bool IsValidMove(Vector2Int pos1, Vector2Int pos2, int[,] data)
    {
        // check xem co hop le khong
        if (pos1.x < 0 || pos1.x >= data.GetLength(0) || pos1.y < 0 || pos1.y >= data.GetLength(1))
            return false;

        if (pos2.x < 0 || pos2.x >= data.GetLength(0) || pos2.y < 0 || pos2.y >= data.GetLength(1))
            return false;

        // Thay đổi hai viên gạch và kiểm tra
        Swap(data, pos1, pos2);
        bool valid = CheckAllMatches(data);
        Swap(data, pos1, pos2);

        return valid;
    }

    protected void Swap(int[,] data, Vector2Int pos1, Vector2Int pos2)
    {
        var temp = data[pos1.x, pos1.y];
        data[pos1.x, pos1.y] = data[pos2.x, pos2.y];
        data[pos2.x, pos2.y] = temp;
    }

    protected bool CheckAllMatches(int[,] data)
    {
        for (int x = 0; x < data.GetLength(0); x++)
        {
            for (int y = 0; y < data.GetLength(1); y++)
            {
                if (IsHorizontalMatch(x, y, data) ||
                    IsVerticalMatch(x, y, data) ||
                    IsLShapeMatch(x, y, data) ||
                    IsTShapeMatch(x, y, data))
                    return true;
            }
        }
        return false;
    }

    private bool CheckMatch(int x, int y, int[,] data)
    {
        return IsHorizontalMatch(x, y, data) ||
                IsVerticalMatch(x, y, data) ||
                IsTShapeMatch(x, y, data) ||
                IsLShapeMatch(x, y, data);
    }

    private bool IsHorizontalMatch(int x, int y, int[,] data)
    {
        return CountHorizontalMatch(x, y, data) >= 3;
    }
    private int CountHorizontalMatch(int x, int y, int[,] data)
    {
        int count = 1; // Đếm từ chính viên đầu tiên
        for (int i = x + 1; i < data.GetLength(0); i++)
        {
            if (data[i, y] == data[x, y])
                count++;
            else
                break;
        }
        return count;
    }

    private bool IsVerticalMatch(int x, int y, int[,] data)
    {
        return CountVerticalMatch(x, y, data) >= 3;
    }
    private int CountVerticalMatch(int x, int y, int[,] data)
    {
        int count = 1; // Đếm từ chính viên đầu tiên
        for (int i = y + 1; i < data.GetLength(1); i++)
        {
            if (data[x, i] == data[x, y])
                count++;
            else
                break;
        }
        return count;
    }

    private bool IsTShapeMatch(int x, int y, int[,] data)
    {
        if (y - 1 >= 0 && y + 1 < data.GetLength(1) &&
            x - 1 >= 0 && x + 1 < data.GetLength(0))
        {
            return data[x, y] == data[x, y - 1] &&
                    data[x, y] == data[x, y + 1] &&
                    (data[x, y] == data[x - 1, y] || data[x, y] == data[x + 1, y]);
        }
        return false;
    }
    private bool IsLShapeMatch(int x, int y, int[,] data)
    {
        return ((x + 2 < data.GetLength(0) && y + 1 < data.GetLength(1) &&
                    data[x, y] == data[x + 1, y] &&
                    data[x, y] == data[x + 2, y] &&
                    data[x, y] == data[x, y + 1]) ||
                (x + 1 < data.GetLength(0) && y + 2 < data.GetLength(1) &&
                    data[x, y] == data[x, y + 1] &&
                    data[x, y] == data[x, y + 2] &&
                    data[x, y] == data[x + 1, y]));
    }

    protected int CalculatePotentialCombo(Vector2Int pos1, Vector2Int pos2, int[,] data)
    {
        // Sao lưu trạng thái ban đầu
        Swap(data, pos1, pos2);

        // Biến lưu điểm combo
        int comboScore = 0;

        // Duyệt qua toàn bộ lưới
        for (int x = 0; x < data.GetLength(0); x++)
        {
            for (int y = 0; y < data.GetLength(1); y++)
            {
                // Tính điểm nếu có match
                // điểm này để ưu tiên tính toán nước đi chứ không phải điểm ăn được
                if (IsHorizontalMatch(x, y, data))
                    comboScore += CountHorizontalMatch(x, y, data) * 10; // Mỗi ô trong combo ngang có giá trị 10 điểm

                if (IsVerticalMatch(x, y, data))
                    comboScore += CountVerticalMatch(x, y, data) * 10; // Mỗi ô trong combo dọc có giá trị 10 điểm

                if (IsTShapeMatch(x, y, data))
                    comboScore += 50; // Match chữ T được thưởng thêm 50 điểm

                if (IsLShapeMatch(x, y, data))
                    comboScore += 50; // Match chữ L được thưởng thêm 50 điểm
            }
        }

        // Khôi phục trạng thái ban đầu
        Swap(data, pos1, pos2);

        return comboScore;
    }

    protected int EvaluateMove(Vector2Int pos1, Vector2Int pos2, int[,] data)
    {
        // Tính điểm combo trực tiếp
        int comboScore = CalculatePotentialCombo(pos1, pos2, data);

        // Ưu tiên combo đặc biệt (T, L, +)
        bool createsSpecialMatch = false;

        Swap(data, pos1, pos2); // Thay đổi để kiểm tra
        if (CheckMatch(pos1.x, pos1.y, data) || CheckMatch(pos2.x, pos2.y, data))
            createsSpecialMatch = true;
        Swap(data, pos1, pos2); // Khôi phục trạng thái ban đầu

        // Tăng điểm nếu tạo combo đặc biệt
        if (createsSpecialMatch)
            comboScore += 100;

        return comboScore;
    }
}
