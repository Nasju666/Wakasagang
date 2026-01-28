using UnityEngine;

public class SpawnerEndTrigger : MonoBehaviour
{
    public BoulderSpawner spawner;
    public bool destroySpawnerOnTrigger = true;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (spawner != null)
            spawner.StopSpawner(destroySpawnerOnTrigger);

       Destroy(gameObject);     
    }
    
}
