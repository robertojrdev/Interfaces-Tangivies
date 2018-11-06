﻿using UnityEngine;

public class GameManager : MonoBehaviour 
{
    public float timeToQuestions = 10;
    private AsteroidPool pool;

    private void Start()
    {
        pool = GetComponent<AsteroidPool>();
    }

    private void StartGame()
    {
        pool.SetSpawnActive(true);
    }
}
