using UnityEngine;

public class Jugador : MonoBehaviour
{
    private MeshRenderer rendererRenderer;


    void Start()
    {

        rendererRenderer = GetComponent<MeshRenderer>();

        rendererRenderer.enabled = false;

    }
}

