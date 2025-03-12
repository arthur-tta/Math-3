using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BruteForce : BaseAI
{

    public override Move GetMove(int[,] data)
    {
        var validMoves = new List<Move>();
        for (int x = 0; x < data.GetLength(0); x++)
        {
            for (int y = 0; y < data.GetLength(1); y++)
            {
                foreach (var dir in directions) // Kiểm tra các hướng (lên, xuống, trái, phải)
                {
                    switch (dir)
                    {
                        case Direction.Left:
                            Vector2Int pos2 = new Vector2Int(x, y - 1);
                            if (IsValidMove(new Vector2Int(x, y), pos2, data)) // Kiểm tra xem có hợp lệ không
                            {
                                validMoves.Add(new Move(new Vector2Int(x, y), dir));
                            }
                            break;
                        case Direction.Right:
                            pos2 = new Vector2Int(x, y + 1);
                            if (IsValidMove(new Vector2Int(x, y), pos2, data)) // Kiểm tra xem có hợp lệ không
                            {
                                validMoves.Add(new Move(new Vector2Int(x, y), dir));
                            }
                            break;
                        case Direction.Top:
                            pos2 = new Vector2Int(x - 1, y);
                            if (IsValidMove(new Vector2Int(x, y), pos2, data)) // Kiểm tra xem có hợp lệ không
                            {
                                validMoves.Add(new Move(new Vector2Int(x, y), dir));
                            }
                            break;
                        case Direction.Bottom:
                            pos2 = new Vector2Int(x + 1, y);
                            if (IsValidMove(new Vector2Int(x, y), pos2, data)) // Kiểm tra xem có hợp lệ không
                            {
                                validMoves.Add(new Move(new Vector2Int(x, y), dir));
                            }
                            break;
                    }

                }
            }
        }
        return validMoves[Random.Range(0, validMoves.Count)]; // Chọn ngẫu nhiên một nước đi hợp lệ
    }
}
