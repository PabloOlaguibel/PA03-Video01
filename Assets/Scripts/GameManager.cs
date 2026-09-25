using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public AudioSource audioSource;
    public TMP_Text collectiblesNumbersText;
    public TMP_Text totalCollectiblesNumbersText;

    private int collectiblesNumber = 0;
    private int totalCollectiblesNumber;
 
    private void Start()
    {
        totalCollectiblesNumber = transform.childCount;

        collectiblesNumbersText.text = "0";
        totalCollectiblesNumbersText.text = totalCollectiblesNumber.ToString();
    }

    private void Update()
    {
        if (transform.childCount <= 0)
        {
            Debug.Log("Win");

            SceneManager.LoadScene(
                SceneManager.GetActiveScene().buildIndex + 1
            );
        }
    }

    public void AddCollectible()
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }

        collectiblesNumber++;

        collectiblesNumbersText.text = collectiblesNumber.ToString();
    }
}