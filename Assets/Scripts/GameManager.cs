using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public TMP_Text collectiblesNumbersText;
    public TMP_Text totalCollectiblesNumbersText;

    private int collectiblesNumber = 0;
    private int totalCollectiblesNumber;

  private void Start()
{
    totalCollectiblesNumber = transform.childCount;

    Debug.Log("Texto superior: " + collectiblesNumbersText);
    Debug.Log("Texto inferior: " + totalCollectiblesNumbersText);

    collectiblesNumbersText.text = "0";
    totalCollectiblesNumbersText.text = totalCollectiblesNumber.ToString();
}

    public void AddCollectible()
    {
        collectiblesNumber++;

        if (collectiblesNumbersText != null)
            collectiblesNumbersText.text = collectiblesNumber.ToString();
    }
}