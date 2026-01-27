using UnityEngine;

public class Relicc : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        RisingFloodd flood = FindObjectOfType<RisingFloodd>();
        if (flood != null)
        {
            flood.StartFloodWithDelay();
        }

        Destroy(gameObject);
    }
}
