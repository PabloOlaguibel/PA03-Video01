using UnityEngine;

public class Jugador : MonoBehaviour
{
    public float speed = 5f;

    private Rigidbody rb;

    private CamaraPrimeraPersona camaraPrimeraPersona;

    private void Awake()
    {

        rb = GetComponent<Rigidbody>();

        camaraPrimeraPersona = GetComponent<CamaraPrimeraPersona>();

    }

    private void Update()
    {

        float moveHorizontal = Input.GetAxis("Horizontal");

        float moveVertical = Input.GetAxis("Vertical");

        float yaw = camaraPrimeraPersona != null ? camaraPrimeraPersona.Yaw : 0f;

        Vector3 direccion = Quaternion.Euler(0f, yaw, 0f) * new Vector3(moveHorizontal, 0f, moveVertical);

        Vector3 movement = new Vector3(direccion.x*speed, rb.linearVelocity.y, direccion.z*speed);

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

