using UnityEngine;

public class EnergyCore : MonoBehaviour
{
    public float rotationSpeed = 100f;
    public float floatingHeight = 0.25f;
    public float floatingSpeed = 2f;
    Vector3 startPosition;

    void Start() { startPosition = transform.position; }
    void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
        transform.position = startPosition + Vector3.up * (Mathf.Sin(Time.time * floatingSpeed) * floatingHeight);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        StageController controller = FindFirstObjectByType<StageController>();
        if (controller != null) controller.RegisterEnergyCore();
        Destroy(gameObject);
    }
}
