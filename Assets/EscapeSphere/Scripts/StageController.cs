using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageController : MonoBehaviour
{
    public string nextScene = "WinScene";
    public float timeLimit = 120f;
    public TMP_Text counterText;
    public TMP_Text timerText;
    public TMP_Text messageText;
    public AudioSource audioSource;
    public AudioClip collectSound;
    public GameObject exitBarrier;

    int total;
    int collected;
    float remaining;
    bool exitReady;

    void Start()
    {
        remaining = timeLimit;
        total = GameObject.FindGameObjectsWithTag("Collectible").Length;
        UpdateCounter();
        if (messageText != null) messageText.text = "Recolecta la energia y encuentra la salida";
    }

    void Update()
    {
        remaining -= Time.deltaTime;
        if (timerText != null)
        {
            int m = Mathf.Max(0, Mathf.FloorToInt(remaining / 60));
            int s = Mathf.Max(0, Mathf.FloorToInt(remaining % 60));
            timerText.text = $"TIEMPO {m:00}:{s:00}";
        }
        if (remaining <= 0) RestartStage();
    }

    public void RegisterEnergyCore()
    {
        collected++;
        if (audioSource != null && collectSound != null) audioSource.PlayOneShot(collectSound);
        UpdateCounter();
        if (collected >= total)
        {
            exitReady = true;
            if (exitBarrier != null) exitBarrier.SetActive(false);
            if (messageText != null) messageText.text = "SALIDA DESBLOQUEADA";
        }
    }

    void UpdateCounter()
    {
        if (counterText != null) counterText.text = $"ENERGIA {collected:00}/{total:00}";
    }

    public void TryFinish()
    {
        if (exitReady || total == 0) SceneManager.LoadScene(nextScene);
        else if (messageText != null) messageText.text = "Aun quedan nucleos de energia";
    }

    public void RestartStage() { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); }
}
