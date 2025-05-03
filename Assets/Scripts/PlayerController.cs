using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    private Rigidbody rb;
    private bool isGrounded = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        float horizontal = Input.GetAxis("Horizontal"); // A/D or ←/→
        float vertical = Input.GetAxis("Vertical");     // W/S or ↑/↓

        Vector3 moveDirection = new Vector3(horizontal, 0f, vertical).normalized;
        Vector3 moveVelocity = moveDirection * moveSpeed;

        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }

    public void Damage(float amount) {
        Debug.Log("Damaged");
    }

}
