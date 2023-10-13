using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectRenderer : MonoBehaviour
{
    // Inspector
    [SerializeField] private BoidBehavior boidPrefab;
    
    // Internal
    private List<BoidBehavior> boids = new();
    private int activeBoidCount;
    private bool isSetup;


    private void Start()
    {
        SetupGameObjects();
    }

    private void Update()
    {
        UpdateGameObjects();
    }

    private void OnEnable()
    {
        SetupGameObjects();
        UpdateGameObjects();
    }

    private void SetupGameObjects()
    {
        if (isSetup) return;
        
        for (int i = 0; i < BoidsManager.MAX_BOIDS_COUNT; i++)
        {
            BoidBehavior boid = Instantiate(boidPrefab, transform);
            boids.Add(boid);
            boid.gameObject.SetActive(false);
        }

        isSetup = true;
    }
    
    /// <summary>
    /// Update all boids.
    /// </summary>
    protected void UpdateGameObjects()
    {
        // Hide/Unhide boids
        if (activeBoidCount != BoidsManager.instance.boidsCount)
        {
            int min = Mathf.Min(activeBoidCount, BoidsManager.instance.boidsCount);
            int max = Mathf.Max(activeBoidCount, BoidsManager.instance.boidsCount);
            
            for (int i = min; i < max; i++)
            {
                boids[i].gameObject.SetActive(i < BoidsManager.instance.boidsCount);
            }
            activeBoidCount = BoidsManager.instance.boidsCount;
        }
        
        // Update from BoidsManager
        for (int i = 0; i < BoidsManager.instance.boidsCount; i++)
        {
            boids[i].UpdateBoid(BoidsManager.instance.boidsData[i]);
        }
    }
}
