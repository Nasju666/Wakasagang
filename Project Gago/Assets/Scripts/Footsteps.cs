using UnityEngine;

public class Footsteps : MonoBehaviour
{
    public AudioSource source;

    public AudioClip[] concrete;
    public AudioClip[] wood;
    public AudioClip[] metal;

    public float walkStepRate = 0.5f;
    public float sprintStepRate = 0.35f;

    float stepTimer;
    PlayerMovement player;

    void Start()
    {
        player = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if (!player.IsGrounded || !player.IsMoving)
            return;

        stepTimer -= Time.deltaTime;
        float rate = player.IsSprinting ? sprintStepRate : walkStepRate;

        if (stepTimer <= 0)
        {
            PlayStep();
            stepTimer = rate;
        }
    }

    void PlayStep()
    {
        if (!Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 2f))
            return;

        AudioClip[] clips = concrete;

        switch (hit.collider.tag)
        {
            case "Wood": clips = wood; break;
            case "Metal": clips = metal; break;
        }

        if (clips.Length == 0) return;

        source.PlayOneShot(clips[Random.Range(0, clips.Length)]);
    }
}
