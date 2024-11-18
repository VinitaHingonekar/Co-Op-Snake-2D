using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeController : MonoBehaviour
{
    private Vector2Int gridPosition;
    private Vector2Int gridMoveDirection;

    private float gridMoveTimer;
    private float gridMoveTimerMax;

    private FoodSpawner foodSpawner;
    
    private List<Transform> snakeBodyParts = new List<Transform>();
    public GameObject bodyPartPrefab;


    private void Awake()
    {
        gridPosition = new Vector2Int(10, 10);
        gridMoveTimerMax = 0.2f;
        gridMoveTimer = gridMoveTimerMax;
        gridMoveDirection = Vector2Int.right;
    }

    private void Start()
    {
        foodSpawner = FindObjectOfType<FoodSpawner>();
        foodSpawner.SpawnFood();

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
            gridMoveTimer -= gridMoveTimerMax;

            // moving the body aorts
            for ( int i = snakeBodyParts.Count -1; i >0; i--)
            {
                snakeBodyParts[i].position = snakeBodyParts[i-1].position;
            }

            if ( snakeBodyParts.Count > 0)
            {
                snakeBodyParts[0].position = transform.position;
            }

            // moving the head

            gridPosition += gridMoveDirection;
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

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.CompareTag("Food"))
        {
            Debug.Log("Snake ate food");
            Destroy(other.gameObject);
            Grow();
            foodSpawner.SpawnFood();

        }
    }

    private void Grow()
    {
        Vector3 newBodyPartPosition = (snakeBodyParts.Count == 0) 
            ? transform.position // spawn at head
            : snakeBodyParts[snakeBodyParts.Count - 1].position; //  spawn at the last position

        GameObject newBodyPart = Instantiate(bodyPartPrefab, newBodyPartPosition, Quaternion.identity);
        snakeBodyParts.Add(newBodyPart.transform);
    }
}