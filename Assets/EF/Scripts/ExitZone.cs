using UnityEngine;

public class ExitZone : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && FinalGameManager.Instance != null && FinalGameManager.Instance.IsExitOpen())
            FinalGameManager.Instance.Win();
    }
}
