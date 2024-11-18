using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    public GameObject foodPrefab;
    [SerializeField] private int girdWidth = 20;
    [SerializeField] private int girdHeight = 20;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnFood()
    {
        Vector2Int foodPosition;

        int x = Random.Range(0, girdWidth);
        int y = Random.Range(0, girdHeight);
        foodPosition = new Vector2Int(x, y);

        Instantiate(foodPrefab, new Vector3(foodPosition.x, foodPosition.y, 0), Quaternion.identity);
    }
}
