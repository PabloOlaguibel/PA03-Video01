using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalGameManager : MonoBehaviour
{
    public static FinalGameManager Instance { get; private set; }
    public TMP_Text counterText;
    public TMP_Text objectiveText;
    public AudioSource audioSource;
    public AudioClip collectSound;
    public GameObject exitDoor;
    public string nextSceneName = "WinScene";

    private int collected;
    private int total;
    private bool doorOpened;

    void Awake() { Instance = this; }

    void Start()
    {
        total = GameObject.FindGameObjectsWithTag("Collectible").Length;
        UpdateUI();
        if (objectiveText != null) objectiveText.text = "Recolecta todas las tarjetas y llega a la salida";
    }

    public void Collect(GameObject item)
    {
        collected++;
        if (audioSource != null && collectSound != null) audioSource.PlayOneShot(collectSound);
        Destroy(item);
        UpdateUI();
        if (collected >= total) OpenExit();
    }

    void UpdateUI()
    {
        if (counterText != null) counterText.text = collected + " / " + total;
    }

    void OpenExit()
    {
        doorOpened = true;
        if (objectiveText != null) objectiveText.text = "Salida desbloqueada";
        if (exitDoor != null) exitDoor.SetActive(false);
    }

    public bool IsExitOpen() { return doorOpened || collected >= total; }
    public void Win() { SceneManager.LoadScene(nextSceneName); }
}
