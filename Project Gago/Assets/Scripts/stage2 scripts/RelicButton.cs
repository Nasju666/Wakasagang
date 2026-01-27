using UnityEngine;

public class RelicButton : MonoBehaviour
{
    [Header("References")]
    public GameObject relicToSpawn;

    bool activated;

    void OnTriggerEnter(Collider other)
    {
        if (activated) return;
        if (!other.CompareTag("Player")) return;

        activated = true;

        if (relicToSpawn != null)
        {
            relicToSpawn.SetActive(true);
        }

        // Optional: button feedback
        Destroy(gameObject);
        // play sound / animation here
    }
}
