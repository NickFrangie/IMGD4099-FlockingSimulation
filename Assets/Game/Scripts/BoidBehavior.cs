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
}
