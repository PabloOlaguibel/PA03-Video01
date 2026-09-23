using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public TMP_Text collectiblesNumbersText;

    private int collectiblesNumber = 0;

    private void Start()
    {
        UpdateCounter();
    }

    public void AddCollectible()
    {
        collectiblesNumber++;
        UpdateCounter();
    }

    private void UpdateCounter()
    {
        collectiblesNumbersText.text = collectiblesNumber.ToString();
    }
}