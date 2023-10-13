using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class BoidsManager : MonoBehaviour
{
    public struct BoidData
    {
        public Vector2 velocity;
        public Vector2 position;
    }

    // Constants
    public const int MAX_BOIDS_COUNT = 60000;
    
    // Singleton
    internal static BoidsManager instance;
    
    // Inspector
    [Header("Time")]
    [Range(0,10)] [SerializeField] internal float simulationTimeScale = 1.0f; // Time scale for simulation
    
    [Header("Simulation")] 
    [SerializeField] private ComputeShader boidsComputeShader; // Compute Shader for Boids simulation
    
    [Header("Boids")]
    [Range(10, MAX_BOIDS_COUNT)] [SerializeField] internal int boidsCount = 10000; // Boids count
    [SerializeField] internal float boidsMaxSpeed = 10.0f;
    [SerializeField] internal float boidsMaxSteering = 1.0f;

    [Header("Controls")] 
    [SerializeField] internal float cohesionRadius = 1.0f; // Radius for applying cohesion to other individuals
    [SerializeField] internal float alignmentRadius = 1.0f; // Radius for applying alignment to other individuals
    [SerializeField] internal float separationRadius = 0.5f; // Radius for applying separation to other individuals
    [SerializeField] internal float cohesionWeight = 0.5f; // Cohesion force appliance weight
    [SerializeField] internal float alignmentWeight = 0.5f; // Alignment force appliance weight
    [SerializeField] internal float separationWeight = 2.0f; // Separation force appliance weight

    [Header("Area")] 
    [SerializeField] internal Vector2 simulationArea = new Vector2(32.0f, 32.0f); // Simulation dimensions
    [SerializeField] internal Vector2 simulationAreaSoftness = new Vector2(8.0f, 8.0f); // Softness of simulation area
    [SerializeField] internal float simulationAreaWeight = 10.0f; // Bounding avoidance weight

    // Internal
    internal ComputeBuffer boidsSteeringBuffer; // Buffer for Boids steering forces values storage
    internal ComputeBuffer boidsDataBuffer; // Buffer storing basic data of Boids (velocity, position, Transform, etc.)

    internal BoidData[] boidsData;
    
    private int boidsSteeringKernelId, boidsDataKernelId; // Kernels for Boids steering and data updates

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;    
        }
    }

    private void Start()
    {
        SetupBuffers();
        SetupKernels();
    }

    private void Update()
    {
        Time.timeScale = simulationTimeScale;
    }

    private void FixedUpdate()
    {
        RunSimulation();
    }

    private void OnDestroy()
    {
        ReleaseBuffers();
        if (instance == this)
        {
            instance = null;
        }
    }

    /// <summary>
    /// Reset simulation.
    /// </summary>
    public void Reset()
    {
        ReleaseBuffers();
        SetupBuffers();
    }
    
    /// <summary>
    /// Setup buffers for Boids simulation.
    /// </summary>
    private void SetupBuffers()
    {
        // Buffers Initialization
        int boidsDataBufferSize = sizeof(float) * 2 + sizeof(float) * 2;
        boidsDataBuffer = new ComputeBuffer(MAX_BOIDS_COUNT, boidsDataBufferSize);
        
        int boidsSteeringBufferSize = sizeof(float) * 2;
        boidsSteeringBuffer = new ComputeBuffer(MAX_BOIDS_COUNT, boidsSteeringBufferSize);

        // Data Initialization
        Vector2[] boidsSteering = new Vector2[MAX_BOIDS_COUNT];
        boidsData = new BoidData[MAX_BOIDS_COUNT];
        for (int i = 0; i < MAX_BOIDS_COUNT; i++)
        {
            boidsData[i].position = Random.insideUnitCircle * 1.0f;
            boidsData[i].velocity = Random.insideUnitCircle * 0.1f;
         
            boidsSteering[i] = Vector2.zero;
        }

        // Data into Buffers
        boidsDataBuffer.SetData(boidsData);   
        boidsSteeringBuffer.SetData(boidsSteering);
    }

    /// <summary>
    /// Setup kernels from Compute Shader.
    /// </summary>
    private void SetupKernels()
    {
        boidsSteeringKernelId = boidsComputeShader.FindKernel("BoidsSteeringCS");
        boidsDataKernelId = boidsComputeShader.FindKernel("BoidsDataCS");
    }
    
    /// <summary>
    /// Release buffers.
    /// </summary>
    private void ReleaseBuffers()
    {
        boidsDataBuffer?.Release();
        boidsDataBuffer = null;
        boidsSteeringBuffer?.Release();
        boidsSteeringBuffer = null;
    }

    /// <summary>
    /// Run the simulation.
    /// </summary>
    private void RunSimulation()
    {
        // Uniforms
        boidsComputeShader.SetInt("boids_count", boidsCount);
        boidsComputeShader.SetFloat("boids_max_speed", boidsMaxSpeed);
        boidsComputeShader.SetFloat("boids_max_steering", boidsMaxSteering);
        
        boidsComputeShader.SetFloat("cohesion_radius", cohesionRadius);
        boidsComputeShader.SetFloat("cohesion_weight", cohesionWeight);
        boidsComputeShader.SetFloat("alignment_radius", alignmentRadius);
        boidsComputeShader.SetFloat("alignment_weight", alignmentWeight);
        boidsComputeShader.SetFloat("separation_radius", separationRadius);
        boidsComputeShader.SetFloat("separation_weight", separationWeight);
        
        boidsComputeShader.SetVector("simulation_area", simulationArea);
        boidsComputeShader.SetVector("simulation_area_softness", simulationAreaSoftness);
        boidsComputeShader.SetFloat("simulation_area_weight", simulationAreaWeight);
   
        boidsComputeShader.SetFloat("delta_time", Time.fixedDeltaTime);

        // Buffers
        boidsComputeShader.SetBuffer(boidsSteeringKernelId, "boids_steering_buffer", boidsSteeringBuffer);
        boidsComputeShader.SetBuffer(boidsSteeringKernelId, "boids_data_buffer", boidsDataBuffer);
        boidsComputeShader.SetBuffer(boidsDataKernelId, "boids_steering_buffer", boidsSteeringBuffer);
        boidsComputeShader.SetBuffer(boidsDataKernelId, "boids_data_buffer", boidsDataBuffer);
        
        // Dispatch
        int threadGroupSize = Mathf.CeilToInt(boidsCount / 64.0f);
        boidsComputeShader.Dispatch(boidsSteeringKernelId, threadGroupSize, 1, 1);
        boidsComputeShader.Dispatch(boidsDataKernelId, threadGroupSize, 1, 1);
        
        // Return
        boidsDataBuffer.GetData(boidsData);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(simulationArea.x, simulationArea.y, 0.0f));
        
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(simulationArea.x + simulationAreaSoftness.x, simulationArea.y + simulationAreaSoftness.y, 0.0f));
    }
}
