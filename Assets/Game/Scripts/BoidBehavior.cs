using UnityEngine;

public class BoidBehavior : MonoBehaviour
{
    /// <summary>
    /// Update boid with new data.
    /// </summary>
    /// <param name="data">Data to update boid with.</param>
    public void UpdateBoid(BoidsManager.BoidData data)
    {
        transform.position = data.position;
        transform.up = data.velocity;
    }
    
    private void OnDrawGizmos()
    {
        if (BoidsManager.instance == null) return;
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, BoidsManager.instance.cohesionRadius);
        
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, BoidsManager.instance.alignmentRadius);
        
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, BoidsManager.instance.separationRadius);
    }
}
