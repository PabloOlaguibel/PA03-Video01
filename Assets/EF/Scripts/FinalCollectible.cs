using UnityEngine;

public class FinalCollectible : MonoBehaviour
{
    public float rotationSpeed = 90f;
    public GameObject collectEffect;

    void Update() { transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f, Space.World); }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (collectEffect != null) Instantiate(collectEffect, transform.position, Quaternion.identity);
        if (FinalGameManager.Instance != null) FinalGameManager.Instance.Collect(gameObject);
        else Destroy(gameObject);
    }
}
