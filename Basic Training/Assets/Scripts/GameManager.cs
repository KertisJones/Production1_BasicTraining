﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic; 

public class GameManager : MonoBehaviour
{
    public float levelStartDelay = 2f;   
    public float turnDelay = 0.1f;
    public int playerFoodPoints = 100; 
    public static GameManager instance = null;  
    [HideInInspector] public bool playersTurn = true;      
    
    private BoardManager boardScript;     
    public int level = 1;         
    private List<Enemy> enemies;    
    private bool enemiesMoving;  
    
    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        
        DontDestroyOnLoad(gameObject);
        enemies = new List<Enemy>();        
        boardScript = GetComponent<BoardManager>();

        InitGame();
    }
    
    void OnLevelWasLoaded(int index)
    {
        level++;
        InitGame();
    }
    
    void InitGame()
    {
        enemies.Clear();
        boardScript.SetupScene(level);

    }
    
    void Update()
    {
        if (playersTurn || enemiesMoving)
            return;
        
        StartCoroutine(MoveEnemies());
    }
    
    public void AddEnemyToList(Enemy script)
    {
        enemies.Add(script);
    }
    
    public void GameOver()
    {
        //levelImage.SetActive(true);
        
        enabled = false;
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
}
