using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class EndGame : MonoBehaviour
{
    public TextMeshProUGUI coins;
    void OnEnable()
    {
        coins.text = "Coins collected: " + Player.totalCoins.ToString();
    }
    public void RestartGame()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
