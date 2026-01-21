using UnityEngine;

public class HeadBob : MonoBehaviour
{
    [Header("Bob Settings")]
    public float walkBobSpeed = 8f;
    public float sprintBobSpeed = 14f;
    public float walkBobAmount = 0.05f;
    public float sprintBobAmount = 0.1f;

    Vector3 startPos;
    float timer;

    PlayerMovement player;

    void Start()
    {
        startPos = transform.localPosition;
        player = GetComponentInParent<PlayerMovement>();
    }

    void Update()
    {
        if (!player) return;

        float speed = player.IsSprinting ? sprintBobSpeed : walkBobSpeed;
        float amount = player.IsSprinting ? sprintBobAmount : walkBobAmount;

        if (player.IsMoving && player.IsGrounded)
        {
            timer += Time.deltaTime * speed;
            float y = Mathf.Sin(timer) * amount;
            transform.localPosition = startPos + Vector3.up * y;
        }
        else
        {
            timer = 0;
            transform.localPosition = Vector3.Lerp(
                transform.localPosition,
                startPos,
                10f * Time.deltaTime
            );
        }
    }
}
