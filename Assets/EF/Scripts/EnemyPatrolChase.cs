using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyPatrolChase : MonoBehaviour
{
    [Header("Referencias")]
    public Transform player;
    public Transform[] waypoints;

    [Header("Comportamiento")]
    public float detectionDistance = 7f;
    public float loseDistance = 10f;
    public float patrolSpeed = 3.5f;
    public float chaseSpeed = 6f;
    public float waitAtPoint = 1f;
    public float captureDistance = 1.1f;

    private NavMeshAgent agent;
    private int waypointIndex;
    private float waitTimer;
    private bool chasing;

    void Awake() { agent = GetComponent<NavMeshAgent>(); }

    void Start()
    {
        if (player == null)
        {
            GameObject found = GameObject.FindGameObjectWithTag("Player");
            if (found != null) player = found.transform;
        }
        agent.speed = patrolSpeed;
        GoToWaypoint();
    }

    void Update()
    {
        if (player == null || !agent.isOnNavMesh) return;
        float distance = Vector3.Distance(transform.position, player.position);
        if (!chasing && distance <= detectionDistance) chasing = true;
        else if (chasing && distance >= loseDistance) { chasing = false; GoToWaypoint(); }

        if (chasing)
        {
            agent.speed = chaseSpeed;
            agent.SetDestination(player.position);
            if (distance <= captureDistance) RestartLevel();
        }
        else Patrol();
    }

    void Patrol()
    {
        if (waypoints == null || waypoints.Length == 0) return;
        agent.speed = patrolSpeed;
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance + 0.15f)
        {
            waitTimer += Time.deltaTime;
            if (waitTimer >= waitAtPoint)
            {
                waitTimer = 0f;
                waypointIndex = (waypointIndex + 1) % waypoints.Length;
                GoToWaypoint();
            }
        }
    }

    void GoToWaypoint()
    {
        if (waypoints != null && waypoints.Length > 0 && agent.isOnNavMesh)
            agent.SetDestination(waypoints[waypointIndex].position);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player")) RestartLevel();
    }

    void RestartLevel() { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow; Gizmos.DrawWireSphere(transform.position, detectionDistance);
        Gizmos.color = Color.red; Gizmos.DrawWireSphere(transform.position, loseDistance);
    }
}
