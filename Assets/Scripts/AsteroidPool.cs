using System;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidPool : MonoBehaviour 
{
    public int poolAmount = 30;
    public float spawnTime = 0.5f;
    public Asteroid asteroidPrefab;
    public float spawnOffset;
    public Vector2 minMaxHeight;
    public Vector2 minMaxSpeed;
    public Vector2 minMaxTorque;

    private Camera cam;
    private List<Asteroid> asteroidsPool;
    private bool isSpawning = false;
    private float spawnTimeCounter;
    private int spawnIndex = 0;

    private void Start()
    {
        cam = FindObjectOfType<Camera>();
        StartPool();
        isSpawning = true;
    }

    private void StartPool()
    {
        asteroidsPool = new List<Asteroid>();

        for (int i = 0; i < poolAmount; i++)
        {
            Asteroid asteroid = Instantiate(
                asteroidPrefab, 
                Vector3.zero, 
                Quaternion.identity, 
                transform).GetComponent<Asteroid>();

            asteroid.gameObject.SetActive(false);
            asteroidsPool.Add(asteroid);
        }
    }

    public void SetSpawnActive(bool active)
    {
        isSpawning = active;
    }

    private void Update()
    {
        spawnTimeCounter -= Time.deltaTime;

        if (isSpawning && spawnTimeCounter <= 0)
            Spawn();            
    }

    private void Spawn()
    {
        spawnTimeCounter = spawnTime;

        if (asteroidsPool == null || asteroidsPool.Count == 0)
            return;

        if (spawnIndex >= asteroidsPool.Count)
            spawnIndex = 0;
        
        asteroidsPool[spawnIndex].gameObject.SetActive(true);

        Vector3 spawnPos = new Vector3();
        spawnPos.x = cam.transform.position.x + spawnOffset;
        spawnPos.y = UnityEngine.Random.Range(minMaxHeight.x, minMaxHeight.y);
        asteroidsPool[spawnIndex].SetPosition(spawnPos);

        Vector3 spawnRot = new Vector3();
        spawnRot.x = UnityEngine.Random.Range(minMaxTorque.x, minMaxTorque.y);
        spawnRot.y = UnityEngine.Random.Range(minMaxTorque.x, minMaxTorque.y);
        spawnRot.z = UnityEngine.Random.Range(minMaxTorque.x, minMaxTorque.y);
        asteroidsPool[spawnIndex].SetRotationSpeed(spawnRot);

        asteroidsPool[spawnIndex].SetSpeed(
            Vector3.left * UnityEngine.Random.Range(minMaxSpeed.x, minMaxSpeed.y));

        spawnIndex++;
    }
}
