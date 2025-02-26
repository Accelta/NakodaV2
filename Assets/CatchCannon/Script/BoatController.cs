using UnityEngine;

public class BoatController : MonoBehaviour
{
    public float moveSpeed = 5f;  // Kecepatan gerak kanan & kiri
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        MoveSideways();
    }

    void MoveSideways()
    {
        float moveInput = Input.GetAxis("Horizontal"); // Tombol A/D atau Arrow Left/Right
        Vector3 moveDirection = transform.right * moveInput * moveSpeed;
        rb.linearVelocity = new Vector3(moveDirection.x, rb.linearVelocity.y, 0); // Hanya bergerak ke kanan & kiri
    }
}
