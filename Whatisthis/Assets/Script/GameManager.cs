using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float levelStartDelay = 2f;
    public float turnDelay = 0.1f;
    public int playerFoodPoints = 100;
    public static GameManager instance = null;

    [HideInInspector]
    public bool playersTurn = true;
    private BoardManager boardManager;
    private int level = 1;
    private List<Enemy> enemies;
    private bool enemiesMoving;    

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        enemies = new List<Enemy>();

        boardManager = GetComponent<BoardManager>();

        InitGame();
    }

    private void InitGame()
    {
        enemies.Clear();

        boardManager.SetupScene(level);
    }

    public void AddEnemyToList(Enemy script)
    {
        enemies.Add(script);
    }      

    void Update()
    {
        if (playersTurn || enemiesMoving)
            return;

        StartCoroutine(MoveEnemies());        
        
    }

    IEnumerator MoveEnemies()
    {
        enemiesMoving = true;
        yield return new WaitForSeconds(turnDelay);

        if (enemies.Count == 0)
        {
            yield return new WaitForSeconds(turnDelay);
        }
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].MoveEnemy();

            yield return new WaitForSeconds(enemies[i].moveTime);
        }

        playersTurn = true;

        enemiesMoving = false;
    }

    public void GameOver()
    {
        enabled = false;
    }
}
