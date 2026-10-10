using UnityEngine;
public class StageExit : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        StageController controller = FindFirstObjectByType<StageController>();
        if (controller != null) controller.TryFinish();
    }
}
