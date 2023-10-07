using UnityEngine;

public class BoidSpawner : MonoBehaviour
{
    public struct Boid
    {
        public Vector2 position;
        public Vector2 velocity;
    }
    
    // Inspector
    [SerializeField] private BoidBehavior boidPrefab;
    [SerializeField] private ComputeShader boidComputerShader;
    
    [Header("Spawning")]
    [SerializeField] private int numBoids = 100;
    [SerializeField] private int numSpawnCircles = 10;
    [SerializeField] private float spawnRadius = 5f;
    [SerializeField] private Vector2 spawnArea = Vector2.one;
    
    [Header("Controls")]
    [SerializeField] private float centeringStrength = 1f;
    [SerializeField] private float separationStrength = 1f;
    [SerializeField] private float matchVelocityStrength = 1f;
    
    // Internal
    private Boid[] boids;
    
    private void Start()
    {
        Initialize();
    }

    private void FixedUpdate()
    {
        // Create Buffer
        int positionSize = sizeof(float) * 2;
        int velocitySize = sizeof(float) * 2;
        int boidSize = positionSize + velocitySize;
        ComputeBuffer inBuffer = new ComputeBuffer(numBoids, boidSize);
        ComputeBuffer outBuffer = new ComputeBuffer(numBoids, boidSize);
        
        // Set Data and Buffers
        inBuffer.SetData(boids);
        boidComputerShader.SetBuffer(0, "in_boids", inBuffer);
        boidComputerShader.SetBuffer(0, "out_boids", outBuffer);
        boidComputerShader.SetInt("boid_count", numBoids);
        
        boidComputerShader.SetFloat("centering_strength", centeringStrength);
        boidComputerShader.SetFloat("separation_strength", separationStrength);
        boidComputerShader.SetFloat("match_velocity_strength", matchVelocityStrength);
        
        // Dispatch
        boidComputerShader.Dispatch(0, Mathf.CeilToInt((float)numBoids / 8), 1, 1);
        
        // Data
        outBuffer.GetData(boids);
        
        // Release
        inBuffer.Release();
        outBuffer.Release();
    }

    /// <summary>
    /// Initialize the boids.
    /// </summary>
    private void Initialize()
    {
        boids = new Boid[numBoids];

        for (int i = 0; i < numSpawnCircles; i++)
        {
            Vector2 spawnPoint = new Vector2(
                Random.Range(-spawnArea.x / 2, spawnArea.x / 2),
                Random.Range(-spawnArea.y / 2, spawnArea.y / 2)
            );
            
            for (int j = 0; j < numBoids / numSpawnCircles; j++)
            {
                CreateBoid(i * (numBoids / numSpawnCircles) + j, spawnPoint);
            }
        }
    }

    /// <summary>
    /// Creates a boid agent.
    /// </summary>
    /// <param name="id">ID of the boid agent.</param>
    /// <param name="spawnPoint">Spawn point of the boid agent.</param>
    private void CreateBoid(int id, Vector2 spawnPoint)
    {
        // Instantiation
        BoidBehavior boid = Instantiate(boidPrefab, transform);
            
        // Data
        Boid data;
        data.position = spawnPoint + Random.insideUnitCircle * Random.Range(0f, spawnRadius);
        data.velocity = Vector2.zero;
        boids[id] = data;
        
        // Values
        boid.spawner = this;
        boid.transform.position = data.position;
        boid.id = id;
    }

    /// <summary>
    /// Returns the specified boid.
    /// </summary>
    /// <param name="id">The ID specifying a boid.</param>
    /// <returns>The specified boid.</returns>
    public Boid GetBoid(int id)
    {
        return boids[id];
    }
    
    private void OnDrawGizmos()
    {
        // Spawn Area
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, spawnArea);
        
        // Spawn Circles
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}
