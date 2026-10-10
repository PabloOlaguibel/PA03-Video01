using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerControllerFinal : MonoBehaviour
{
    public float speed = 10f;
    public float maxSpeed = 12f;
    private Rigidbody rb;
    private Vector2 input;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }

    void Update() { input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized; }

    void FixedUpdate()
    {
        rb.AddForce(new Vector3(input.x, 0f, input.y) * speed, ForceMode.Acceleration);
        Vector3 horizontal = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        if (horizontal.magnitude > maxSpeed)
        {
            horizontal = horizontal.normalized * maxSpeed;
            rb.linearVelocity = new Vector3(horizontal.x, rb.linearVelocity.y, horizontal.z);
        }
    }
}
