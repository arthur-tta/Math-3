using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Cell : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Vector2Int position {  get; private set; }

    private Vector2 firstTouchPosition, finalTouchPosition;
    private float swipeThreshold = 0.5f;

    public void SetUp(Vector2Int position)
    {
        this.position = position;
    }


    public void OnPointerUp(PointerEventData eventData)
    {
        finalTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        CalculateSwipeDirection();
        //Debug.Log("Up");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        firstTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Debug.Log("Down");
    }


    private void CalculateSwipeDirection()
    {
        Vector2 difference = finalTouchPosition - firstTouchPosition;

        if (Mathf.Abs(difference.x) > swipeThreshold || Mathf.Abs(difference.y) > swipeThreshold)
        {
            if(Mathf.Abs(difference.x) > Mathf.Abs(difference.y))
            {
                if(difference.x > 0)
                {
                    SwapDiamond(Direction.Right);
                }
                else
                {
                    SwapDiamond(Direction.Left);
                }
            }
            else
            {
                if(difference.y > 0)
                {
                    SwapDiamond(Direction.Top);
                }
                else
                {
                    SwapDiamond(Direction.Bottom);
                }
            }      
        }
    }

    private void SwapDiamond(Direction direction)
    {
        Move move = new Move(position, direction);
        Board.Instance.CheckSwap(move);
    }

   
}
