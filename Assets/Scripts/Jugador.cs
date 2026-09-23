using UnityEngine;

public class Jugador : MonoBehaviour
{
    public float speed = 5f;

    private Rigidbody rb;

    private void Awake()
    {

        rb = GetComponent<Rigidbody>();

    }

    private void Update()
    {

        float moveHorizontal = Input.GetAxis("Horizontal");

        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal*speed, rb.linearVelocity.y, moveVertical*speed);

        rb.linearVelocity = movement;

    }

    private void OnCollisionEnter(Collision collision)

    {
        if (collision.transform.CompareTag("Collectible"))
        {
            Destroy(collision.gameObject);

        }
    }

}

