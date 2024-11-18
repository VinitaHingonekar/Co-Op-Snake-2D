using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    public GameObject foodPrefab;
    [SerializeField] private int girdWidth = 20;
    [SerializeField] private int girdHeight = 20;

    private SnakeController snakeController;
    
    // Start is called before the first frame update
    void Start()
    {
        snakeController = FindObjectOfType<SnakeController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnFood()
    {
        Vector2Int foodPosition;
        do{
            int x = Random.Range(0, girdWidth);
            int y = Random.Range(0, girdHeight);
            foodPosition = new Vector2Int(x, y);
        }
        while (IsOccupiedBySnake(foodPosition));

        Instantiate(foodPrefab, new Vector3(foodPosition.x, foodPosition.y, 0), Quaternion.identity);
    }

    private bool IsOccupiedBySnake(Vector2Int position)
    {
        if (snakeController.GetSnakeHeadPosition() == position)
            return true;
        
        foreach(Transform bodyPart in snakeController.GetSnakeBodyParts())
        {
            if(Vector2Int.RoundToInt(bodyPart.position) == position)
                return true;
        }

        return false;
    }
}
