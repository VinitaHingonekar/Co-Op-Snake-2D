using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SnakeController : MonoBehaviour
{
    public Vector2Int gridSize = new Vector2Int(20, 20);

    private Vector2Int headPosition;
    private Vector2Int moveDirection;

    private float moveTimer;
    private float moveTimerMax;

    private FoodSpawner foodSpawner;
    
    private List<Transform> snakeBodyParts = new List<Transform>();
    public GameObject bodyPartPrefab;

    private int decreaseLenght = 2;

    private int score;
    private int foodScore = 10;

    // power up varaibles
    bool hasShield = false;
    private int scoreMultiplier = 1;

    private void Awake()
    {
        headPosition = new Vector2Int(10, 10);
        moveTimerMax = 0.1f;
        moveTimer = moveTimerMax;
        moveDirection = Vector2Int.right;
    }

    private void Start()
    {
        foodSpawner = FindObjectOfType<FoodSpawner>();

    }

    void Update()
    {
        HandleInput();
        Move();

    }

    private void HandleInput()
    {
        if(Input.GetKeyDown(KeyCode.W) && moveDirection != Vector2Int.down)
            moveDirection = Vector2Int.up;
        else if(Input.GetKeyDown(KeyCode.S) && moveDirection != Vector2Int.up)
           moveDirection =  Vector2Int.down;
        else if(Input.GetKeyDown(KeyCode.A) && moveDirection != Vector2Int.right)
            moveDirection = Vector2Int.left;
        else if(Input.GetKeyDown(KeyCode.D) && moveDirection != Vector2Int.left)
            moveDirection = Vector2Int.right;
    }

    private void Move()
    {
        moveTimer += Time.deltaTime;
        if(moveTimer >= moveTimerMax)
        {
            moveTimer -= moveTimerMax;

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
            headPosition += moveDirection;
            
            // screen wrapping 
            headPosition = WrapGridPosition(headPosition);

            transform.position = new Vector3(headPosition.x, headPosition.y);
            transform.eulerAngles = new Vector3 (0, 0, GetAngleFromDirection(moveDirection));

            // checking collision with body
            if(IsOccupiedBySnake(headPosition))
            {
                Debug.Log("Die");
                Death();
            }
        }
    }

    private Vector2Int WrapGridPosition(Vector2Int position)
    {
        position.x = (position.x + gridSize.x) % gridSize.x;
        position.y = (position.y + gridSize.y) % gridSize.y;
        return position;
    }

    private float GetAngleFromDirection(Vector2Int direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if(angle < 0) angle += 360;
        return angle;
    }

    public bool IsOccupiedBySnake(Vector2Int position)
    {
        // if (headPosition == position)
        //     return true;
        
        foreach(Transform bodyPart in snakeBodyParts)
        {
            if(Vector2Int.RoundToInt(bodyPart.position) == position)
                return true;
        }
        return false;
    }

    private void Death()
    {
        if(hasShield)
        {
            Debug.Log("Shield used");
            hasShield = false;
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            score = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.CompareTag("MassGainer"))
        {
            // Debug.Log("Snake ate gainer");
            Destroy(other.gameObject);
            Grow();
            AddScore(foodScore);
        }
        else if(other.CompareTag("MassBurner"))
        {
            // Debug.Log("Snake ate burner");
            Destroy(other.gameObject);
            Shrink(decreaseLenght);
            AddScore(-foodScore);
        }
        else if(other.CompareTag("PowerUp"))
        {
            Debug.Log("Snake got a power up");
            Destroy(other.gameObject);
            Shrink(decreaseLenght);
            AddScore(-foodScore);
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

    private void Shrink(int lenght)
    {
        for(int i=0; i < lenght; i++)
        {
            if (snakeBodyParts.Count > 0)
            {
                Transform lastPart = snakeBodyParts[snakeBodyParts.Count - 1];
                snakeBodyParts.Remove(lastPart);
                Destroy(lastPart.gameObject);
            }
            else
            {
                Debug.Log("Cannot Shrink");
                break;
            }
        }
    }

    private void AddScore(int points)
    {
        score += points * scoreMultiplier;
        Debug.Log("Score: " + score);
    }

    public Vector2Int GetSnakeHeadPosition()
    {
        return headPosition;
    }

    public List<Transform> GetSnakeBodyParts()
    {
        return snakeBodyParts;
    }

    public int GetSnakeSize()
    {
        return 1+ snakeBodyParts.Count;
    }


}