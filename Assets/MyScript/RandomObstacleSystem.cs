using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomObstacleSystem : MonoBehaviour
{
    int index;
    private float difficulty = 1;

    private float lowRange = 0.0f;
    private float mediumRange = 0.1f;
    private float highRange = 0.4f;

    [SerializeField] private int obstacleCount = 5;

    public void Start()
    {
        RandomObstacle();
    }

    public void FixedUpdate()
    {
        //increasing difficult obstacles over time
        difficulty -= 0.01f;
    }

    public void RandomObstacle()
    {
        //Obstacle difficulty according to probability
        if (difficulty >= lowRange && difficulty < mediumRange)
        {
            ActiveObstacle(2,0);
        }

        else if (difficulty >= mediumRange && difficulty < highRange)
        {
            ActiveObstacle(1,-1);
        }
        else
        {
            ActiveObstacle(0,-2);
        }
    }

    void ActiveObstacle(int minRandom ,int limit)
    {
        transform.GetChild(index).gameObject.SetActive(false);

        index = Random.Range(minRandom, obstacleCount + limit);

        if (transform.GetChild(index) != null)
        {
            transform.GetChild(index).gameObject.SetActive(true);
        }
    }
}
