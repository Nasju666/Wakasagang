using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class RisingFloodd : MonoBehaviour
{
    public float riseSpeed = 0.5f;
    public float startDelay = 2.5f;

    bool isActive;

    void Update()
    {
        if (!isActive) return;
        transform.Translate(Vector3.up * riseSpeed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void StartFloodWithDelay()
    {
        StartCoroutine(StartFloodCoroutine());
    }

    IEnumerator StartFloodCoroutine()
    {
        Debug.Log("LAVA RISING!");
        yield return new WaitForSeconds(startDelay);
        isActive = true;
    }
}
