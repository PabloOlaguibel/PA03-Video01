using UnityEngine;

public class Jugador : MonoBehaviour
{
    public Rigidbody rigidbody;


    void Start()
    {
        rigidbody.useGravity = false;

        Debug.Log("Hola Mundo");


    }
}

