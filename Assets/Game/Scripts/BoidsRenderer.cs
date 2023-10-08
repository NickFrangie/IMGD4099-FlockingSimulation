using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoidsManager))]
public class BoidsRenderer : MonoBehaviour
{
    // Inspector
    [Header("References")]
    [SerializeField] private BoidBehavior boidPrefab;
    
    // Internal
    private List<BoidBehavior> boids = new();
    private bool isInitialized = false;
    
    // References
    private BoidsManager boidsManager;

    private void Awake()
    {
        boidsManager = GetComponent<BoidsManager>();
    }

    private void Update()
    {
        if (!isInitialized) SpawnBoids();
        UpdateBoids();
    }

    private void SpawnBoids()
    {
        int boidsCount = boidsManager.boidsCount;
        for (int i = 0; i < boidsCount; i++)
        {
            BoidBehavior boid = Instantiate(boidPrefab, transform);
            Vector2 randomPosition = new Vector2(
                Random.Range(-boidsManager.simulationArea.x, boidsManager.simulationArea.x),
                Random.Range(-boidsManager.simulationArea.y, boidsManager.simulationArea.y)
            );
            boid.transform.position = randomPosition;
            boids.Add(boid);
        }
        
        isInitialized = true;
    }
    
    /// <summary>
    /// Update all boids.
    /// </summary>
    private void UpdateBoids()
    {
        for (int i = 0; i < boids.Count; i++)
        {
            boids[i].UpdateBoid(boidsManager.boidsData[i]);
        }
    }
}
