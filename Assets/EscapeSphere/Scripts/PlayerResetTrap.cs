using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerResetTrap : MonoBehaviour
{
    void OnCollisionEnter(Collision hit)
    {
        if(hit.transform.CompareTag("Player")) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
