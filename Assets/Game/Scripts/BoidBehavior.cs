using UnityEngine;

/// <summary>
/// Behaviors for boid agents.
/// </summary>
public class BoidBehavior : MonoBehaviour
{
    // Internal
    internal int id = 0;
    
    // References
    internal BoidSpawner spawner;

    
    public void Update()
    {
        BoidSpawner.Boid data = spawner.GetBoid(id);
        transform.position = data.position;
        transform.rotation = Quaternion.LookRotation(Vector3.forward, data.velocity);
    }
}
