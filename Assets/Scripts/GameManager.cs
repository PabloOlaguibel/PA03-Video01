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


    collectiblesNumbersText.text = "0";
    totalCollectiblesNumbersText.text = totalCollectiblesNumber.ToString();
}

void Update()
{
    if (transform.childCount <= 0)
    {
        Debug.Log("Win");
    }
}

    public void AddCollectible()
    {
        collectiblesNumber++;

        if (collectiblesNumbersText != null)
            collectiblesNumbersText.text = collectiblesNumber.ToString();
    }
}