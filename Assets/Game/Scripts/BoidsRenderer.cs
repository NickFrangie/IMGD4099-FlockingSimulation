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
    private int activeBoidCount;
    
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
        activeBoidCount = boidsManager.boidsCount;
        for (int i = 0; i < activeBoidCount; i++)
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
        // Hide/Unhide boids
        if (activeBoidCount != boidsManager.boidsCount)
        {
            int min = Mathf.Min(activeBoidCount, boidsManager.boidsCount);
            int max = Mathf.Max(activeBoidCount, boidsManager.boidsCount);
            
            for (int i = min; i < max; i++)
            {
                boids[i].gameObject.SetActive(i < boidsManager.boidsCount);
            }
            activeBoidCount = boidsManager.boidsCount;
        }
        
        // Update from BoidsManager
        for (int i = 0; i < boidsManager.boidsCount; i++)
        {
            boids[i].UpdateBoid(boidsManager.boidsData[i]);
        }
    }
}
