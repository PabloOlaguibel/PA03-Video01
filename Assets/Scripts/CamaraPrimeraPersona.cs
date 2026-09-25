using UnityEngine;

public class CamaraPrimeraPersona : MonoBehaviour
{
    public KeyCode teclaCambiar = KeyCode.V;
    public bool empezarEnPrimeraPersona = false;
    public float sensibilidad = 2f;
    public float alturaOjo = 1f;
    public float fovPrimeraPersona = 75f;
    public float anguloMinimo = -80f;
    public float anguloMaximo = 80f;

    private Camera camara;
    private Renderer[] renderersJugador;
    private Vector3 posicionOriginalCamara;
    private Quaternion rotacionOriginalCamara;
    private float fovOriginalCamara;
    private bool primeraPersona;
    private float yaw;
    private float pitch;

    public bool PrimeraPersona
    {
        get { return primeraPersona; }
    }

    public float Yaw
    {
        get { return yaw; }
    }

    private void Start()
    {
        camara = Camera.main;

        renderersJugador = GetComponentsInChildren<Renderer>(true);

        if (camara != null)
        {
            posicionOriginalCamara = camara.transform.position;
            rotacionOriginalCamara = camara.transform.rotation;
            fovOriginalCamara = camara.fieldOfView;
        }

        if (empezarEnPrimeraPersona)
        {
            ActivarPrimeraPersona();
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(teclaCambiar))
        {
            Alternar();
        }
    }

    private void LateUpdate()
    {
        if (!primeraPersona || camara == null)
        {
            return;
        }

        yaw += Input.GetAxis("Mouse X") * sensibilidad;

        pitch = Mathf.Clamp(
            pitch - Input.GetAxis("Mouse Y") * sensibilidad,
            anguloMinimo,
            anguloMaximo
        );

        Transform transformCamara = camara.transform;

        transformCamara.position = transform.position + Vector3.up * alturaOjo;
        transformCamara.rotation = Quaternion.Euler(pitch, yaw, 0f);
    }

    public void Alternar()
    {
        if (primeraPersona)
        {
            ActivarCamaraCenital();
        }
        else
        {
            ActivarPrimeraPersona();
        }
    }

    private void ActivarPrimeraPersona()
    {
        if (camara == null)
        {
            return;
        }

        primeraPersona = true;
        pitch = 0f;

        camara.fieldOfView = fovPrimeraPersona;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        for (int i = 0; i < renderersJugador.Length; i++)
        {
            renderersJugador[i].enabled = false;
        }
    }

    private void ActivarCamaraCenital()
    {
        primeraPersona = false;
        yaw = 0f;
        pitch = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        for (int i = 0; i < renderersJugador.Length; i++)
        {
            renderersJugador[i].enabled = true;
        }

        if (camara != null)
        {
            camara.transform.position = posicionOriginalCamara;
            camara.transform.rotation = rotacionOriginalCamara;
            camara.fieldOfView = fovOriginalCamara;
        }
    }
}
