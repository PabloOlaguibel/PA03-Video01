using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScene : MonoBehaviour
{
    private void Start()
    {
        Invoke("LoadMainMenu", 3);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
} 