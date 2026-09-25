using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Enemigo : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private Transform playerTransform;

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        GameObject jugador = GameObject.FindGameObjectWithTag("Player");

        if (jugador != null)
        {
            playerTransform = jugador.transform;
        }
    }

    private void Update()
    {
        if (playerTransform != null)
        {
            navMeshAgent.destination = playerTransform.position;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            SceneManager.LoadScene(
                SceneManager.GetActiveScene().buildIndex
            );
        }
    }
}