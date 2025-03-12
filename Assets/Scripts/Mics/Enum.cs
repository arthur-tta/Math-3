using System.Numerics;
using UnityEngine;

public enum Direction
{
    Left,
    Right, 
    Top, 
    Bottom

}
public class Move
{
    public Vector2Int position;
    public Direction direction;

    public Move(Vector2Int position, Direction direction)
    {
        this.position = position;
        this.direction = direction;
    }
}