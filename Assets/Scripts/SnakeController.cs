using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeController : MonoBehaviour
{
    private Vector2Int gridPosition;
    private Vector2Int gridMoveDirection;

    private float gridMoveTimer;
    private float gridMoveTimerMax;


    private void Awake()
    {
        gridPosition = new Vector2Int(0, 0);
        gridMoveTimerMax = 0.5f;
        gridMoveTimer = gridMoveTimerMax;
        gridMoveDirection = Vector2Int.right;
    }

    void Update()
    {
        HandleInput();
        Move();
    }

    private void HandleInput()
    {
        if(Input.GetKeyDown(KeyCode.W) && gridMoveDirection != Vector2Int.down)
            gridMoveDirection = Vector2Int.up;
        else if(Input.GetKeyDown(KeyCode.S) && gridMoveDirection != Vector2Int.up)
           gridMoveDirection =  Vector2Int.down;
        else if(Input.GetKeyDown(KeyCode.A) && gridMoveDirection != Vector2Int.right)
            gridMoveDirection = Vector2Int.left;
        else if(Input.GetKeyDown(KeyCode.D) && gridMoveDirection != Vector2Int.left)
            gridMoveDirection = Vector2Int.right;
    }

    private void Move()
    {
        gridMoveTimer += Time.deltaTime;
        if(gridMoveTimer >= gridMoveTimerMax)
        {
            gridPosition += gridMoveDirection;
            gridMoveTimer -= gridMoveTimerMax;
            
            transform.position = new Vector3(gridPosition.x, gridPosition.y);
            transform.eulerAngles = new Vector3 (0, 0, GetAngleFromDirection(gridMoveDirection));
        }
    }

    private float GetAngleFromDirection(Vector2Int direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if(angle < 0) angle += 360;
        return angle;
    }
}