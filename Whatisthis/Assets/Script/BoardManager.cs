using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using System;

public class BoardManager : MonoBehaviour
{
    [SerializeField]
    public class Count
    {
        public int minimum;
        public int maximum;

        public Count(int min, int max)
        {
            minimum = min;
            maximum = max;
        }
    }

    public int colums = 8;
    public int rows = 8;

    public Count wallCount = new Count(5, 9);
    public Count foodCount = new Count(1, 5);

    public GameObject exit;
    public GameObject[] floorTiles;
    public GameObject[] wallTiles;
    public GameObject[] foodTiles;
    public GameObject[] enemyTiles;
    public GameObject[] otherWallTiles;

    private Transform boadHoler;
    private List<Vector3> gridPositions = new List<Vector3>();

    void InitialiseList()
    {
        gridPositions.Clear();
        for (int x = 0; x < colums; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                gridPositions.Add(new Vector3(x, y, 0));
            }
        }
    }

    void BoardSetup()
    {
        boadHoler = new GameObject("Board").transform;
        for (int x = -1; x < colums + 1; x++)
        {
            for (int y = -1; y < rows + 1; y++)
            {
                GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];

                if (x == -1 || x == colums || y == -1 || y == rows)
                    toInstantiate = otherWallTiles[Random.Range(0, otherWallTiles.Length)];
                GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity);

                instance.transform.SetParent(boadHoler);
            }
        }
    }
    public void SetupScene(int level)
    {
        BoardSetup();

        InitialiseList();

        
    }

    void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum)
    {
        int objectCount = Random.Range(minimum, maximum);

        for (int i = 0; i < objectCount; i++)
        {
            Vector3 randomPosition = RandomPosition();
            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];

            Instantiate(tileChoice, randomPosition, Quaternion.identity);
        }
    }

    Vector3 RandomPosition()
    {
        int randIdx = Random.Range(0, gridPositions.Count);

        Vector3 randPosition = gridPositions[randIdx];
        gridPositions.RemoveAt(randIdx);

        return randPosition;
    }
}
