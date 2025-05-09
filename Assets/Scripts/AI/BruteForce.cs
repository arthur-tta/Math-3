﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BruteForce : BaseAI
{
    public override Move GetMove(int[,] data)
    {
        int maxScore = 0; // Điểm cao nhất
        Move bestMove = null; // nuoc di toi uu nhat

        for (int x = 0; x < data.GetLength(0); x++)
        {
            for (int y = 0; y < data.GetLength(1); y++)
            {
                foreach (var dir in directions)
                {
                    switch (dir)
                    {
                        case Direction.Left:
                            Vector2Int pos2 = new Vector2Int(x, y - 1);
                            if (IsValidMove(new Vector2Int(x, y), pos2, data)) // Kiểm tra xem có hợp lệ không
                            {
                                int score = CalculatePotentialCombo(new Vector2Int(x, y), pos2, data); // Tính điểm
                                if (score >= maxScore) // Nếu điểm cao hơn, cập nhật nước đi tốt nhất
                                {
                                    maxScore = score;
                                    bestMove = new Move(new Vector2Int(x, y), dir);
                                }
                            }
                            break;
                        case Direction.Right:
                            pos2 = new Vector2Int(x, y + 1);
                            if (IsValidMove(new Vector2Int(x, y), pos2, data)) // Kiểm tra xem có hợp lệ không
                            {
                                int score = CalculatePotentialCombo(new Vector2Int(x, y), pos2, data); // Tính điểm
                                if (score >= maxScore) // Nếu điểm cao hơn, cập nhật nước đi tốt nhất
                                {
                                    maxScore = score;
                                    bestMove = new Move(new Vector2Int(x, y), dir);
                                }
                            }
                            break;
                        case Direction.Top:
                            pos2 = new Vector2Int(x - 1, y);
                            if (IsValidMove(new Vector2Int(x, y), pos2, data)) // Kiểm tra xem có hợp lệ không
                            {
                                int score = CalculatePotentialCombo(new Vector2Int(x, y), pos2, data); // Tính điểm
                                if (score >= maxScore) // Nếu điểm cao hơn, cập nhật nước đi tốt nhất
                                {
                                    maxScore = score;
                                    bestMove = new Move(new Vector2Int(x, y), dir);
                                }
                            }
                            break;
                        case Direction.Bottom:
                            pos2 = new Vector2Int(x + 1, y);
                            if (IsValidMove(new Vector2Int(x, y), pos2, data)) // Kiểm tra xem có hợp lệ không
                            {
                                int score = CalculatePotentialCombo(new Vector2Int(x, y), pos2, data); // Tính điểm
                                if (score >= maxScore) // Nếu điểm cao hơn, cập nhật nước đi tốt nhất
                                {
                                    maxScore = score;
                                    bestMove = new Move(new Vector2Int(x, y), dir);
                                }
                            }
                            break;
                    }
                }
            }
        }

        return bestMove; // Trả về nước đi tốt nhấ
    }
}
